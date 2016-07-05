using System;
using System.Collections.Generic;

namespace DapperRepositoryBase
{
    /// <summary>
    ///Interface that contains the operations Read.
    /// </summary>
    /// <typeparam name="T">Entity Poco</typeparam>
    public interface IRepositoryBase<out T>  where T : class
    {
        /// <summary>
        /// Get all based on table Name.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Get();
        /// <summary>
        /// Get a single element based on Key = columnName and Value = columnValun.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        T Get(KeyValuePair<string, int> value);
        /// <summary>
        /// Get a single element based on Key = columnName and Value = columnValun.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        T Get(KeyValuePair<string, string> value);
        /// <summary>
        /// Get a Enumerable of elements based on Key = columnName and Value = columnValun.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IEnumerable<T> GetList(KeyValuePair<string, int> value);
        /// <summary>
        ///  Get a Enumerable of elements based on Key = columnName and Value = columnValun.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IEnumerable<T> GetList(KeyValuePair<string, string> value);
        /// <summary>
        /// Get count of specific table.
        /// </summary>
        /// <returns></returns>
        int Count();
    }
}