using System;

namespace HealthChecks.Azure.IoTHubDeviceProvisioningService
{
    public class IoTHubDeviceProvisioningServiceOptions
    {
        internal string ConnectionString { get; private set; }
        internal bool EnrollmentReadCheck { get; set; }
        internal bool EnrollmentWriteCheck { get; set; }
        internal string EnrollmentReadQuery { get; private set; }
        internal Func<string> EnrollmentGroupWriteIdFactory { get; private set; }

        public IoTHubDeviceProvisioningServiceOptions AddConnectionString(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            return this;
        }
        public IoTHubDeviceProvisioningServiceOptions AddEnrollmentReadCheck(string query = "SELECT * FROM enrollments")
        {
            EnrollmentReadCheck = true;
            EnrollmentReadQuery = query;
            return this;
        }
        public IoTHubDeviceProvisioningServiceOptions AddEnrollmentWriteCheck(Func<string> enrollmentGroupIdFactory = null)
        {
            EnrollmentWriteCheck = true;
            EnrollmentGroupWriteIdFactory = enrollmentGroupIdFactory ?? (() => "health-check-write-enrollment-group-id");
            return this;
        }
    }
}