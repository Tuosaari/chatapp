using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Lib.Azure.Storage
{
    /// <summary>
    /// Generic helper interface for CRUD operations against a single Azure storage table
    /// </summary>
    public interface ITableStorageClient
    {
        /// <summary>
        /// Insert or replace <see cref="ITableEntity"/> in the table
        /// </summary>
        /// <param name="entity"><see cref="ITableEntity"/> to insert or replace</param>
        Task InsertOrReplace(ITableEntity entity);

        /// <summary>
        /// Get <see cref="IEnumerable{ITableEntity}"/> by partition key
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="ITableEntity"/> to get</typeparam>
        /// <param name="partitionKey">Partition key to query with</param>
        /// <returns><see cref="IEnumerable{ITableEntity}"/> containing all entities with given partition key</returns>
        Task<IEnumerable<T>> GetByPartitionKey<T>(string partitionKey) where T : ITableEntity, new();

        /// <summary>
        /// Get <see cref="ITableEntity"/> by partition and row key
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="ITableEntity"/> to get</typeparam>
        /// <param name="partitionKey">Partition key to query with</param>
        /// <param name="rowKey">Row key to query with</param>
        /// <returns><see cref="ITableEntity"/> matching the query or null if entity was not found</returns>
        Task<T> GetByPartitionAndRowKey<T>(string partitionKey, string rowKey) where T : ITableEntity, new();

        /// <summary>
        /// Get all <see cref="ITableEntity">entities</see> in table
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="ITableEntity"/> to get</typeparam>
        /// <param name="maxCount">Maximum count of entities to return, set to 0 to retrieve all. Defaults to 0</param>
        /// <returns><see cref="IEnumerable{ITableEntity}"/> containing all entities in table</returns>
        Task<IEnumerable<T>> GetAll<T>(int maxCount = 0) where T : ITableEntity, new();

        /// <summary>
        /// Remove single <see cref="ITableEntity"/> specified by partition and row key
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="ITableEntity"/> to get</typeparam>
        /// <param name="partitionKey">Partition key to use in delete</param>
        /// <param name="rowKey">Row key to use in delete</param>
        /// <returns>Removed <see cref="ITableEntity"/> or null, if entity was not found</returns>
        Task<T> RemoveEntity<T>(string partitionKey, string rowKey) where T : ITableEntity, new();

        /// <summary>
        /// Clear table by removing all entities in it. Note: Does not remove the table
        /// </summary>
        Task RemoveAll();
    }
}
