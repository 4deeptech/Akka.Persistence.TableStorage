using Akka.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Persistence.TableStorage
{
    public class TableStorageSettings
    {
        private Dictionary<string, CloudStorageAccount> _storageAccounts;

        public TableStorageSettings(IList<string> connectionStrings)
        {
            _storageAccounts = new Dictionary<string, CloudStorageAccount>();
            for(int i=0;i<connectionStrings.Count;i++)
            {
                _storageAccounts.Add(i.ToString("X1"), CloudStorageAccount.Parse(connectionStrings[i]));
            }
        }

        public CloudStorageAccount GetAccount(string key)
        {
            if (_storageAccounts.Keys.Contains(key))
            {
                return _storageAccounts[key];
            }
            else return _storageAccounts["0"];
        }

        public CloudTableClient GetClient(string id)
        {
            return GetAccount(id.Substring(0, 1)).CreateCloudTableClient();
        }

        public IEnumerable<CloudStorageAccount> GetStorageAccounts()
        {
            return _storageAccounts.Values;
        }
    }
    /// <summary>
    /// Configuration settings representation targeting Azure TableStorage journal actor.
    /// </summary>
    public class JournalSettings 
    {
        /// <summary>
        /// Name of the table corresponding to snapshot store.
        /// </summary>
        public string TableName { get; private set; }

        public IList<string> ConnectionStrings = new List<string>();

        private TableStorageSettings _settings = null;

        public JournalSettings(Config config)
        {
            if (config == null) throw new ArgumentNullException("config", "Table Storage journal settings cannot be initialized, because required HOCON section couldn't be found");
            TableName = config.GetString("table-name");
            ConnectionStrings = config.GetStringList("connection-strings");
            _settings = new TableStorageSettings(ConnectionStrings);
        }

        public CloudTableClient GetClient(string id)
        {
            return _settings.GetClient(id);
        }
    }

    /// <summary>
    /// Configuration settings representation targeting Azure Table Storage snapshot store actor.
    /// </summary>
    public class SnapshotStoreSettings
    {
        /// <summary>
        /// Name of the table corresponding to snapshot store.
        /// </summary>
        public string TableName { get; private set; }

        public IList<string> ConnectionStrings = new List<string>();

        private TableStorageSettings _settings = null;

        public SnapshotStoreSettings(Config config)
        {
            if (config == null) throw new ArgumentNullException("config", "Azure Table Storage snapshot store settings cannot be initialized, because required HOCON section couldn't be found");
            TableName = config.GetString("table-name");
            ConnectionStrings = config.GetStringList("connection-strings");
            _settings = new TableStorageSettings(ConnectionStrings);
        }

        public CloudTableClient GetClient(string id)
        {
            return _settings.GetClient(id);
        }
    }
}
