using System.Collections.Generic;

namespace DapperRepositoryBase
{
    /// <summary>
    /// Interface that contains the operations Create, Update and Delete.
    /// </summary>
    /// <typeparam name="T">Entity Poco</typeparam>
    public interface IRepositoryOperationBase<T> : IRepositoryBase<T> where T : class
    {
        /// <summary>
        /// Add a single element. 
        /// If the element has a field identity have to add attribute [Ignore] and [Identity].
        /// </summary>
        /// <param name="item">Entity.</param>
        T Add(T item);
        /// <summary>
        /// Add a Enumerable of elements.
        /// If the element has a field identity have to add attribute [Ignore] and [Identity].
        /// </summary>
        /// <param name="item">Entity.</param>
        void Add(IEnumerable<T> item);

        void InsertBulk(IEnumerable<T> items);
        
        /// <summary>
        /// Update a single element. 
        /// If the element has a field identity have to add attribute [Ignore] and [Identity].
        /// </summary>
        /// <param name="item">Entity.</param>
        T Update(T item);
        /// <summary>
        /// Update a Enumerable of elements.
        /// If the element has a field identity have to add attribute [Ignore] and [Identity].
        /// </summary>
        /// <param name="item">Entity.</param>
        void Update(IEnumerable<T> item);
        /// <summary>
        /// Delete a single element. 
        /// If the element has a field identity have to add attribute [Ignore] and [Identity].
        /// </summary>
        /// <param name="item">Entity.</param>
        void Delete(IEnumerable<T> item);
        /// <summary>
        /// Delete a Enumerable of elements.
        /// If the element has a field identity have to add attribute [Ignore] and [Identity].
        /// </summary>
        /// <param name="item">Entity.</param>
        void Delete(T item);
    }
}