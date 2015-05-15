using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Persistence.TableStorage
{
    internal static class TableStorageInitializer
    {
        /// <summary>
        /// Initializes journal tables according 'table-name' 
        ///  values provided in 'akka.persistence.journal.table-storage' config.
        /// </summary>
        internal static void CreateJournalTables(IEnumerable<string> storageAccountConnectionStrings, string tableName)
        {
            foreach(string connectionString in storageAccountConnectionStrings)
            {
                CreateTable(connectionString,tableName);
            }
        }

        /// <summary>
        /// Initializes snapshot store related table according to 'table-name' 
        /// values provided in 'akka.persistence.snapshot-store.table-storage' config.
        /// </summary>
        internal static void CreateSnapshotStoreTables(IEnumerable<string> storageAccountConnectionStrings, string tableName)
        {
            foreach (string connectionString in storageAccountConnectionStrings)
            {
                CreateTable(connectionString, tableName);
            }
        }

        internal static void CreateTable(string connectionString,string tableName)
        {
            CloudTableClient tableClient = CloudStorageAccount.Parse(connectionString).CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
        }
    }
}
