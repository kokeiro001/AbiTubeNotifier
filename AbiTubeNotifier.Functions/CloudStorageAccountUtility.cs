using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;

namespace AbiTubeNotifier.Functions
{
    static class CloudStorageAccountUtility
    {
        public static CloudStorageAccount GetDefaultStorageAccount() =>
            CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("AzureWebJobsStorage"));
    }
}
