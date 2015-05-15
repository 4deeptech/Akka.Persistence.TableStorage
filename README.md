# Akka.Persistence.TableStorage
Azure Table Storage implementation for Akka.Persistence (Akka.Net)

## Technical Overview

This implementation makes use of Azure Table Storage and is set up to be able to be configured using HOCON style configuration.
See the Tests for samples which uses the local development storage emulator.

It is also set up to use a sharded set of storage accounts with up 16 accounts.  The PersistenceId is used to determine which
storage account connection is used.  Since the Journal and the Snapshot Store are separate, technically you could use 32 different 
accounts, 16 for Journal and 16 for Snapshots.  If you do not want to shard then use the same storage account connection string in all
16 slots as seen in the tests.

At the moment, snapshots do not automagically detect if they are too large for an Azure Table Storage row and make use of blob storage
but my in the future since just under a 1MB snapshot is the upper limit with this as implemented.
