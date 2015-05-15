using Akka.Actor;
using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Akka.Persistence.TableStorage
{
    public class TableStorageJournalSettings : JournalSettings
    {
        public const string ConfigPath = "akka.persistence.journal.table-storage";

        /// <summary>
        /// Flag determining in in case of event journal table missing, it should be automatically initialized.
        /// </summary>
        public bool AutoInitialize { get; private set; }

        public TableStorageJournalSettings(Config config)
            : base(config)
        {
            AutoInitialize = config.GetBoolean("auto-initialize");
        }
    }

    public class TableStorageSnapshotSettings : SnapshotStoreSettings
    {
        public const string ConfigPath = "akka.persistence.snapshot-store.table-storage";

        /// <summary>
        /// Flag determining in case of snapshot store table missing, it should be automatically initialized.
        /// </summary>
        public bool AutoInitialize { get; private set; }

        public TableStorageSnapshotSettings(Config config)
            : base(config)
        {
            AutoInitialize = config.GetBoolean("auto-initialize");
        }
    }

    /// <summary>
    /// An actor system extension initializing support for Table Storage persistence layer.
    /// </summary>
    public class TableStoragePersistenceExtension : IExtension
    {
        /// <summary>
        /// Journal-related settings loaded from HOCON configuration.
        /// </summary>
        public readonly TableStorageJournalSettings JournalSettings;

        /// <summary>
        /// Snapshot store related settings loaded from HOCON configuration.
        /// </summary>
        public readonly TableStorageSnapshotSettings SnapshotStoreSettings;

        public TableStoragePersistenceExtension(ExtendedActorSystem system)
        {
            system.Settings.InjectTopLevelFallback(TableStoragePersistence.DefaultConfiguration());

            JournalSettings = new TableStorageJournalSettings(system.Settings.Config.GetConfig(TableStorageJournalSettings.ConfigPath));
            SnapshotStoreSettings = new TableStorageSnapshotSettings(system.Settings.Config.GetConfig(TableStorageSnapshotSettings.ConfigPath));

            if (JournalSettings.AutoInitialize)
            {
                TableStorageInitializer.CreateJournalTables(JournalSettings.ConnectionStrings, JournalSettings.TableName);
            }

            if (SnapshotStoreSettings.AutoInitialize)
            {
                TableStorageInitializer.CreateSnapshotStoreTables(SnapshotStoreSettings.ConnectionStrings, SnapshotStoreSettings.TableName);
            }
        }
    }

    /// <summary>
    /// Singleton class used to setup Azure Table Storage backend for akka persistence plugin.
    /// </summary>
    public class TableStoragePersistence : ExtensionIdProvider<TableStoragePersistenceExtension>
    {
        public static readonly TableStoragePersistence Instance = new TableStoragePersistence();

        /// <summary>
        /// Initializes a Table Storage persistence plugin inside provided <paramref name="actorSystem"/>.
        /// </summary>
        public static void Init(ActorSystem actorSystem)
        {
            Instance.Apply(actorSystem);
        }

        private TableStoragePersistence() { }

        /// <summary>
        /// Creates an actor system extension for akka persistence Table Storage support.
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public override TableStoragePersistenceExtension CreateExtension(ExtendedActorSystem system)
        {
            return new TableStoragePersistenceExtension(system);
        }

        /// <summary>
        /// Returns a default configuration for akka persistence table storage journals and snapshot stores.
        /// </summary>
        /// <returns></returns>
        public static Config DefaultConfiguration()
        {
            return ConfigurationFactory.FromResource<TableStoragePersistence>("Akka.Persistence.TableStorage.table-storage.conf");
        }
    }
}
