using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableStorage.Persistence
{
    public static class ShardedTableHelper
    {
        private static Dictionary<string, CloudStorageAccount> _storageAccounts;

        public static CloudStorageAccount GetAccount(string key)
        {
            if (_storageAccounts.Keys.Contains(key))
            {
                return _storageAccounts[key];
            }
            else return _storageAccounts["0"];
        }

        public static CloudTableClient GetClient(Guid id)
        {
            return GetAccount(id.ShardId16()).CreateCloudTableClient();
        }

        public static CloudTableClient GetClient(string id)
        {
            return GetAccount(id.Substring(0, 1)).CreateCloudTableClient();
        }

        static ShardedTableHelper()
        {
            _storageAccounts = new Dictionary<string, CloudStorageAccount>();
            _storageAccounts.Add("0", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionString0")));
            _storageAccounts.Add("1", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionString1")));
            _storageAccounts.Add("2", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionString2")));
            _storageAccounts.Add("3", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionString3")));
            _storageAccounts.Add("4", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionString4")));
            _storageAccounts.Add("5", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionString5")));
            _storageAccounts.Add("6", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionString6")));
            _storageAccounts.Add("7", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionString7")));
            _storageAccounts.Add("8", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionString8")));
            _storageAccounts.Add("9", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionString9")));
            _storageAccounts.Add("a", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionStringA")));
            _storageAccounts.Add("b", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionStringB")));
            _storageAccounts.Add("c", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionStringC")));
            _storageAccounts.Add("d", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionStringD")));
            _storageAccounts.Add("e", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionStringE")));
            _storageAccounts.Add("f", CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionStringF")));
        }
    }

    public static class GuidExtension
    {
        public static string ShardId16(this Guid g)
        {
            return g.ToString().Substring(0, 1);
        }

        public static string ToPartitionKey(this Guid g)
        {
            return g.ToString("n");
        }

        public static string ToRowKey(this Guid g)
        {
            return g.ToString("n");
        }
    }
}
