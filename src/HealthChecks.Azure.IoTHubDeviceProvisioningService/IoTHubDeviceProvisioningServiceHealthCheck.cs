﻿using System;
using System.IO;
using System.Reflection;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Provisioning.Service;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.Azure.IoTHubDeviceProvisioningService
{
    public class IoTHubDeviceProvisioningServiceHealthCheck : IHealthCheck
    {
        private readonly IoTHubDeviceProvisioningServiceOptions _options;
        public IoTHubDeviceProvisioningServiceHealthCheck(IoTHubDeviceProvisioningServiceOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (_options.EnrollmentWriteCheck)
                {
                    await ExecuteEnrollmentWriteCheckAsync(cancellationToken);
                }
                else if (_options.EnrollmentReadCheck)
                {
                    await ExecuteEnrollmentReadCheckAsync(cancellationToken);
                }
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
        private async Task ExecuteEnrollmentReadCheckAsync(CancellationToken cancellationToken)
        {
            using (var client = ProvisioningServiceClient.CreateFromConnectionString(_options.ConnectionString))
            {
                var specification = new QuerySpecification(_options.EnrollmentReadQuery);
                var enrollmentGroupQuery = client.CreateEnrollmentGroupQuery(specification, 1, cancellationToken);
                await enrollmentGroupQuery.NextAsync();
            }
        }
        private async Task ExecuteEnrollmentWriteCheckAsync(CancellationToken cancellationToken)
        {
            using (var client = ProvisioningServiceClient.CreateFromConnectionString(_options.ConnectionString))
            {
                var enrollmentGroupId = _options.EnrollmentGroupWriteIdFactory();
                EnrollmentGroup enrollmentGroup = null;
                try
                {
                    enrollmentGroup = await client.GetEnrollmentGroupAsync(enrollmentGroupId, cancellationToken);
                }
                catch (ProvisioningServiceClientHttpException exc) when (exc.StatusCode == HttpStatusCode.NotFound)
                {
                }

                Attestation attestation = null;
                var assembly = typeof(IoTHubDeviceProvisioningServiceHealthCheck).Assembly;
                using (var stream = assembly.GetManifestResourceStream("HealthChecks.Azure.IoTHubDeviceProvisioningService.health-checks.default.cer"))
                {
                    byte[] bytes = new byte[stream.Length];
                    await stream.ReadAsync(bytes, (int) stream.Length, (int) stream.Length, cancellationToken);
                    
                    var certificate = new X509Certificate2();
                    certificate.Import(
                        bytes,
                        string.Empty,
                        X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.DefaultKeySet |
                        X509KeyStorageFlags.UserKeySet |
                        X509KeyStorageFlags.UserProtected |
                        X509KeyStorageFlags.PersistKeySet);

                    attestation = X509Attestation.CreateFromClientCertificates(certificate);
                }

                // in default implementation of configuration enrollment group id equals "health-check-write-enrollment-group-id"
                // if in previous health check enrollment group were not removed -- try remove it
                // if in previous health check enrollment group were added and removed -- try create and remove it
                if (enrollmentGroup == null)
                {
                    enrollmentGroup = new EnrollmentGroup(enrollmentGroupId, attestation);
                    await client.CreateOrUpdateEnrollmentGroupAsync(enrollmentGroup, cancellationToken);
                }

                await client.DeleteEnrollmentGroupAsync(enrollmentGroupId, cancellationToken);
            }
        }
    }
}