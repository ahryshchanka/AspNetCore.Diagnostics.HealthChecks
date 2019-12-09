using System;

namespace HealthChecks.Azure.IoTHubDeviceProvisioningService
{
    public class IoTHubDeviceProvisioningServiceOptions
    {
        internal string ConnectionString { get; private set; }
        internal bool ServiceConnectionCheck { get; private set; }
        internal bool EnrollmentReadCheck { get; set; }
        internal bool EnrollmentWriteCheck { get; set; }
        internal bool RegistrationStatusReadCheck { get; private set; }
        internal bool RegistrationStatusWriteCheck { get; private set; }

        public IoTHubDeviceProvisioningServiceOptions AddConnectionString(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            return this;
        }
        public IoTHubDeviceProvisioningServiceOptions AddServiceConnectionCheck()
        {
            ServiceConnectionCheck = true;
            return this;
        }
        public IoTHubDeviceProvisioningServiceOptions AddEnrollmentReadCheck()
        {
            EnrollmentReadCheck = true;
            return this;
        }
        public IoTHubDeviceProvisioningServiceOptions AddEnrollmentWriteCheck()
        {
            EnrollmentWriteCheck = true;
            return this;
        }
        public IoTHubDeviceProvisioningServiceOptions AddRegistrationStatusReadCheck()
        {
            RegistrationStatusReadCheck = true;
            return this;
        }
        public IoTHubDeviceProvisioningServiceOptions AddRegistrationStatusWriteCheck()
        {
            RegistrationStatusWriteCheck = true;
            return this;
        }
    }
}