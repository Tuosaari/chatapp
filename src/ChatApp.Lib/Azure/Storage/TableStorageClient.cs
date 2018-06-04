using ChatApp.Lib.General;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ChatApp.Lib.Azure.Storage
{
    /// <summary>
    /// Options (connection string and table name) used by <see cref="TableStorageClient"/>
    /// </summary>
    public class TableStorageClientOptions
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
    }

    /// <summary>
    /// Default implementation of <see cref="ITableStorageClient"/>
    /// Also implements <see cref="IInitializable"/> to support table creation if required
    /// </summary>
    public class TableStorageClient : ITableStorageClient, IInitializable
    {
        private readonly TableStorageClientOptions _options;
        private readonly CloudStorageAccount _storageAccount;

        private CloudTable Table
        {
            get
            {
                var client = _storageAccount.CreateCloudTableClient();

                var tableServicePoint = ServicePointManager.FindServicePoint(_storageAccount.TableEndpoint);
                tableServicePoint.UseNagleAlgorithm = true;
                tableServicePoint.ConnectionLimit = 100;

                var requestOptions = new TableRequestOptions
                {
                    RetryPolicy = new LinearRetry(TimeSpan.FromMilliseconds(200), 3),
                    MaximumExecutionTime = TimeSpan.FromSeconds(60)
                };

                return client.GetTableReference(_options.TableName);
            }
        }

        public TableStorageClient(TableStorageClientOptions options)
        {
            _options = options;
            _storageAccount = CloudStorageAccount.Parse(options.ConnectionString);
        }

        /// <inheritdoc />
        public async Task InsertOrReplace(ITableEntity entity)
        {
            if (entity == null) {
                throw new ArgumentNullException("Entity cannot be null", nameof(entity));
            }

            await Table.ExecuteAsync(TableOperation.InsertOrReplace(entity));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetByPartitionKey<T>(string partitionKey) where T : ITableEntity, new()
        {
            var query = new TableQuery<T>();
            query = query.Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            return await Table.ExecuteQuerySegmentedAsync(query, new TableContinuationToken()).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<T> GetByPartitionAndRowKey<T>(string partitionKey, string rowKey)
            where T : ITableEntity, new()
        {
            var query = new TableQuery<T>();
            query = query.Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            var entities = await Table.ExecuteQuerySegmentedAsync(query, new TableContinuationToken())
                .ConfigureAwait(false);
            return entities.SingleOrDefault();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAll<T>(int maxCount = 0) where T : ITableEntity, new()
        {
            var result = new List<T>();
            TableContinuationToken token = null;

            do {
                var sequence = await Table
                    .ExecuteQuerySegmentedAsync(new TableQuery<T>(), token)
                    .ConfigureAwait(false);
                token = sequence.ContinuationToken;
                result.AddRange(sequence);

                if (maxCount > 0 && result.Count > maxCount) {
                    result = result.Take(maxCount).ToList();
                    break;
                }
            } while (token != null);

            return result;
        }

        /// <inheritdoc />
        public async Task<T> RemoveEntity<T>(string partitionKey, string rowKey) where T : ITableEntity, new()
        {
            var entity = await GetByPartitionAndRowKey<T>(partitionKey, rowKey).ConfigureAwait(false);

            if (entity != null) {
                await Table.ExecuteAsync(TableOperation.Delete(entity)).ConfigureAwait(false);
                return entity;
            }

            return default(T);
        }

        /// <inheritdoc />
        public async Task RemoveAll()
        {
            var entities = await GetAll<DynamicTableEntity>().ConfigureAwait(false);
            foreach (var entity in entities) {
                await Table.ExecuteAsync(TableOperation.Delete(entity)).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task Initialize()
        {
            await Table.CreateIfNotExistsAsync();
        }
    }
}