using System;
using System.Threading;
using System.Threading.Tasks;
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
                if (_options.ServiceConnectionCheck)
                {
                    await ExecuteServiceConnectionCheckAsync(cancellationToken);
                }

                if (_options.EnrollmentWriteCheck)
                {
                    await ExecuteEnrollmentWriteCheckAsync(cancellationToken);
                }
                else if (_options.EnrollmentReadCheck)
                {
                    await ExecuteEnrollmentWriteCheckAsync(cancellationToken);
                }

                if (_options.RegistrationStatusWriteCheck)
                {
                    await ExecuteRegistryWriteCheckAsync(cancellationToken);
                }
                else if (_options.RegistrationStatusReadCheck)
                {
                    await ExecuteRegistryReadCheckAsync(cancellationToken);
                }

                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
        private async Task ExecuteServiceConnectionCheckAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        private async Task ExecuteEnrollmentReadCheckAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        private async Task ExecuteEnrollmentWriteCheckAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        private async Task ExecuteRegistryReadCheckAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        private async Task ExecuteRegistryWriteCheckAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}