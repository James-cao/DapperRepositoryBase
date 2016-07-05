using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;

namespace DapperRepositoryBase.PlPgSql
{
    /// <summary>
    /// Repository base that are based on Micro-ORM Dapper, it has all the artifacts necessary to do CRUD on POSTGRE.
    /// </summary>
    /// <typeparam name="T">Entity POCO</typeparam>
    public abstract class RepositoryBase<T> : IRepositoryOperationBase<T> where T : class
    {
        /// <summary>
        /// Current DbConnection.
        /// </summary>
        protected IDbConnection DbConnection { get; private set; }
        /// <summary>
        /// Current commandTimeout.
        /// </summary>
        protected int CommandTimeout { get; private set; }

        protected RepositoryBase(IDbConnection dbConnection)
        {
            if (dbConnection == null) throw new ArgumentNullException("dbConnection");
            DbConnection = dbConnection;

        }

        /// <summary>
        /// Get all item about a specific table.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> Get()
        {
            return Get<T>();
        }

        /// <summary>
        /// Get a specific element over the condition "Where [Key] = [Value]".
        /// </summary>
        /// <param name="value">Condition Where [Key] = [Value].</param>
        /// <returns>The entity found.</returns>
        public virtual T Get(KeyValuePair<string, string> value)
        {
            return Get<T>(value);
        }


        /// <summary>
        /// Get a specific element over the condition "Where [Key] = [Value]".
        /// </summary>
        /// <param name="value">Condition Where [Key] = [Value].</param>
        /// <returns>The entity found.</returns>
        public virtual T Get(KeyValuePair<string, int> value)
        {
            return Get<T>(value);
        }

        /// <summary>
        /// Get all elements over the condition "Where [Key] = [Value]".
        /// </summary>
        /// <param name="value">Condition Where [Key] = [Value].</param>
        /// <returns>List the entity found.</returns>
        public virtual IEnumerable<T> GetList(KeyValuePair<string, int> value)
        {
            return GetList<T>(value);
        }

        /// <summary>
        /// Get all elements over the condition "Where [Key] = [Value]".
        /// </summary>
        /// <param name="value">Condition Where [Key] = [Value].</param>
        /// <returns>List the entity found.</returns>
        public virtual IEnumerable<T> GetList(KeyValuePair<string, string> value)
        {
            return GetList<T>(value);
        }


        /// <summary>
        /// Get a number of rows.
        /// </summary>
        /// <returns></returns>
        public virtual int Count()
        {
            return Count<T>();
        }

        /// <summary>
        /// Insert a new row on the table.
        /// </summary>
        /// <param name="item">Entity POCO to insert.</param>
        /// <returns>New id identity assigned.</returns>
        public virtual T Add(T item)
        {
            var query = BuilderOfInsert(item);
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            return DbConnection.Query<T>(query, commandTimeout: CommandTimeout).FirstOrDefault();
        }
        /// <summary>
        /// Insert a list new row on the table.
        /// </summary>
        /// <param name="item">List of Entities POCOs to insert.</param>
        public virtual void Add(IEnumerable<T> item)
        {
            var queries = BuilderOfInsert(item);
            var query = string.Join(Environment.NewLine, queries);
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            DbConnection.Execute(query, commandTimeout: CommandTimeout);
        }

        /// <summary>
        /// Modify a row of the table.
        ///  </summary>
        /// <param name="item">Entity Poco that have to has specified the identity property with IdentityAttribute.</param>
        public virtual T Update(T item)
        {
            var query = BuilderOfUpdate(item);
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            return DbConnection.Query<T>(query, commandTimeout: CommandTimeout).FirstOrDefault();
        }
        /// <summary>
        /// Modify a list of row of the table.
        ///  </summary>
        /// <param name="item">List Entity Poco that have to has specified the identity property with IdentityAttribute.</param>
        public virtual void Update(IEnumerable<T> item)
        {
            var queries = BuilderOfUpdate(item);
            var query = String.Join(Environment.NewLine, queries);
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            DbConnection.Query<int>(query, commandTimeout: CommandTimeout);
        }

        /// <summary>
        /// Remove a row of the table.
        /// </summary>
        /// <param name="item">Entity Poco that have to has specified the identity property with IdentityAttribute.</param>
        public virtual void Delete(T item)
        {
            var query = BuilderOfDelete(item);
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            DbConnection.Execute(query, commandTimeout: CommandTimeout);
        }

        /// <summary>
        /// Remove a list the row of the table.
        /// </summary>
        /// <param name="item">List Entity Poco that have to has specified the identity property with IdentityAttribute.</param>
        public virtual void Delete(IEnumerable<T> item)
        {
            var queries = BuilderOfDelete(item);
            var query = String.Join(Environment.NewLine, queries);
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            DbConnection.Execute(query, commandTimeout: CommandTimeout);
        }

        #region -Helper

        /// <summary>
        /// Get all.
        /// </summary>
        /// <typeparam name="TX">Entity POCO</typeparam>
        /// <returns></returns>
        protected IEnumerable<TX> Get<TX>()
        {
            var tablename = GetTableName(typeof(TX));
            var query = string.Format("SELECT * FROM {0}", tablename);
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            return DbConnection.Query<TX>(query, commandTimeout: CommandTimeout);
        }
        /// <summary>
        /// Get a specific entity over the condition "Where [Key] = [Value]".
        /// <typeparam name="TX"></typeparam>
        /// <param name="value">Condition Where [Key] = [Value].</param>
        /// <returns></returns>
        /// </summary>
        protected TX Get<TX>(KeyValuePair<string, string> value)
        {
            var tableName = GetTableName(typeof(TX));
            var query = string.Format("SELECT * FROM {0} WHERE {1} = '{2}'", tableName, value.Key, value.Value);
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            var result = DbConnection.Query<TX>(query).FirstOrDefault();
            return result;
        }
        /// <summary>
        /// Get a specific element over the condition "Where [Key] = [Value]".
        /// </summary>
        /// <param name="value">Condition Where [Key] = [Value].</param>
        /// <returns>The entity found.</returns>
        public virtual TX Get<TX>(KeyValuePair<string, int> value)
        {
            var tableName = GetTableName(typeof(TX));
            var query = string.Format("SELECT * FROM {0} WHERE {1} = {2}", tableName, value.Key, value.Value);
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            var result = DbConnection.Query<TX>(query, new { value.Key, value.Value }).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Get all elements over the condition "Where [Key] = [Value]".
        /// </summary>
        /// <param name="value">Condition Where [Key] = [Value].</param>
        /// <returns>List the entity found.</returns>
        protected IEnumerable<TX> GetList<TX>(KeyValuePair<string, int> value)
        {
            var tableName = GetTableName(typeof(TX));
            var query = string.Format("SELECT * FROM {0} WHERE {1} = {2}", tableName, value.Key, value.Value);
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            var result = DbConnection.Query<TX>(query, new { value.Key, value.Value }).ToList();
            return result;
        }

        /// <summary>
        /// Get all elements over the condition "Where [Key] = [Value]".
        /// </summary>
        /// <param name="value">Condition Where [Key] = [Value].</param>
        /// <returns>List the entity found.</returns>
        protected IEnumerable<TX> GetList<TX>(KeyValuePair<string, string> value)
        {
            var tableName = GetTableName(typeof(TX));
            var query = string.Format("SELECT * FROM {0} WHERE {1} = '{2}'", tableName, value.Key, value.Value);
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            var result = DbConnection.Query<TX>(query).ToList();
            return result;
        }

        /// <summary>
        /// Get a number of rows.
        /// </summary>
        /// <returns></returns>
        protected int Count<TX>()
        {
            var tableName = GetTableName(typeof(TX));
            var query = string.Format("SELECT Count(*) FROM {0} ", tableName);
            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();
            var result = DbConnection.Query<int>(query).FirstOrDefault();
            return result;
        }


        #region -Const
        /// <summary>
        /// SQL Statement that return Scope Identity.
        /// </summary>
        private const string ReturnRow = "RETURNING*";
        #endregion


        /// <summary>
        /// Build of IN SQL condition based on enumerable of values.
        /// </summary>
        /// <param name="values">Agglomerate terms.</param>
        /// <returns> IN (values) </returns>
        protected string BuilderOfIn(IEnumerable<string> values)
        {
            return string.Format("IN ('{0}')", string.Join("','", values));
        }

        /// <summary>
        /// Build of IN SQL condition based on enumerable of values.
        /// </summary>
        /// <param name="values">Agglomerate terms.</param>
        /// <returns></returns>
        protected string BuilderOfIn(IEnumerable<int> values)
        {
            return string.Format("({0})", string.Join(",", values));
        }

        /// <summary>
        /// Get the tablename form attribute [TableName] if is defiend, otherwise return name of entity poco. 
        /// </summary>
        /// <param name="type">Entity Poco.</param>
        /// <returns></returns>
        protected string GetTableName(Type type)
        {
            if (Attribute.IsDefined(type, typeof(TableName)))
            {
                var attr = Attribute.GetCustomAttribute(type, typeof(TableName));
                return ((TableName)attr).TableNameWithScheme;
            }
            return type.Name;
        }
        private static bool IsNullable(PropertyInfo obj, out Type underlyingType)
        {
            underlyingType = Nullable.GetUnderlyingType(obj.PropertyType);
            var type = obj.PropertyType;
            if (!type.IsValueType) return true;
            if (underlyingType != null) return true;
            underlyingType = obj.PropertyType;
            return false;
        }
        private static string CreateValueQuery(PropertyInfo property, object propertyValue, bool comma)
        {
            if (propertyValue == null) return "null";
            if (!Attribute.IsDefined(property, typeof(DataTypeAttribute))) return string.Empty;

            var attribute = property.GetCustomAttribute<DataTypeAttribute>();
            var queryValues = new StringBuilder();
            Type typeCustom;
            IsNullable(property, out typeCustom);

            switch (attribute.DataType)
            {
                case DataType.Timestamp:
                    if (typeCustom == typeof(DateTime))
                    {
                        var value = ((DateTime)propertyValue);
                        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        var time = Convert.ToDouble((value - epoch).TotalSeconds);
                        queryValues.Append(string.Format("TO_TIMESTAMP({0}) ", time));
                    }
                    break;

                case DataType.Integer:
                case DataType.Serial:
                case DataType.Double:
                    queryValues.Append(string.Format("{0} ", propertyValue));
                    break;
                case DataType.Boolean:
                    queryValues.Append(string.Format("{0} ", propertyValue));
                    break;
                default:
                    queryValues.Append(string.Format("'{0}' ", propertyValue));
                    break;
            }

            queryValues.Insert(0, comma ? "," : string.Empty);
            return queryValues.ToString();
        }

        /// <summary>
        /// Build a Insert SQL Statement based TX entity.
        /// </summary>
        /// <typeparam name="TX">Entity Poco</typeparam>
        /// <param name="entity">Object to insert.</param>
        /// <param name="returnObj"></param>
        /// <returns>SQL Statement.</returns>
        protected string BuilderOfInsert<TX>(TX entity, bool returnObj = true)
        {
            var itemType = entity.GetType();
            var properties = itemType.GetProperties().Where(p => !Attribute.IsDefined(p, typeof(Ignore)) &&
                !Attribute.IsDefined(p, typeof(Identity))).ToArray();

            var tableName = GetTableName(typeof(TX));

            var queryParameter = new StringBuilder();
            var queryValues = new StringBuilder();
            for (var index = 0; index < properties.Length; index++)
            {
                var comma = index > 0;
                var property = properties[index];
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity, null);

                queryParameter.Append(string.Format("{1}{0}", propertyName, comma ? "," : string.Empty));
                queryValues.Append(CreateValueQuery(property, propertyValue, comma));
            }

            var query = new StringBuilder(string.Format("INSERT INTO {0} ({1})  VALUES ({2}) {3} ", tableName, queryParameter, queryValues, returnObj ? ReturnRow : string.Empty));

            return query.ToString();
        }

        private static string Condiction<TX>(PropertyInfo propertyInfo, TX entity)
        {
            var propertyName = propertyInfo.Name;
            var propertyValue = propertyInfo.GetValue(entity, null);
            var value = CreateValueQuery(propertyInfo, propertyValue, false);
            return string.Format("WHERE {0} = {1} ", propertyName, value);
        }

        /// <summary>
        /// Build a Insert SQL Statement based TX entity.
        /// </summary>
        /// <typeparam name="TX">Entity Poco</typeparam>
        /// <param name="entity">Object to insert.</param>
        /// <param name="returnObj"></param>
        /// <returns>SQL Statement.</returns>
        protected string BuilderOfUpdate<TX>(TX entity, bool returnObj = true)
        {
            var itemType = entity.GetType();
            var properties = itemType.GetProperties().Where(p => !Attribute.IsDefined(p, typeof(Ignore)) &&
              !Attribute.IsDefined(p, typeof(Identity))).ToArray();
            var identity = itemType.GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(Identity)));

            var tableName = GetTableName(typeof(TX));

            var queryParameter = new StringBuilder();
            var queryValues = new StringBuilder();
            var queryCondition = Condiction(identity, entity);
            for (var index = 0; index < properties.Length; index++)
            {
                var comma = index > 0;
                var property = properties[index];
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity, null);

                queryParameter.Append(string.Format("{1}{0}", propertyName, comma ? "," : string.Empty));
                queryValues.Append(CreateValueQuery(property, propertyValue, comma));

            }

            var query = new StringBuilder(string.Format("UPDATE {0} SET ({1}) = ({2}) {3} {4}", tableName, queryParameter, queryValues, queryCondition, returnObj ? ReturnRow : string.Empty));

            return query.ToString();
        }
        /// <summary>
        /// Build a multiple Insert SQL Statement basado on TX entity.
        /// </summary>
        /// <typeparam name="TX">Entity Poco</typeparam>
        /// <param name="enumerable">Enumearble entities.</param>
        /// <returns>Enumerable SQL Statement</returns>
        protected IEnumerable<string> BuilderOfInsert<TX>(IEnumerable<TX> enumerable)
        {
            return enumerable.Select(p => BuilderOfInsert(p, false));
        }

        /// <summary>
        /// Build a multiple Update  SQL Statement based on TX entity.
        /// </summary>
        /// <typeparam name="TX">Entity Poco</typeparam>
        /// <param name="enumerable">Enumearble entities.</param>
        /// <returns>Enumerable SQL Statement</returns>
        protected IEnumerable<string> BuilderOfUpdate<TX>(IEnumerable<TX> enumerable)
        {
            return enumerable.Select(p => BuilderOfUpdate(p, false)).ToList();
        }
        /// <summary>
        /// Build a Delete SQL Statement based on TX entity.
        /// </summary>
        /// <param name="entity">Entity Poco will be removed.</param>
        /// <returns>SQL Statement.</returns>
        protected string BuilderOfDelete<TX>(TX entity)
        {
            var itemType = entity.GetType();
            var properties = itemType.GetProperties();

            var property = properties.FirstOrDefault(p => Attribute.IsDefined(p, typeof(Identity)));
            if (property == null) return null;

            var propertyName = property.Name;
            var propertyValue = property.GetValue(entity, null);
            var tableName = GetTableName(itemType);
            var queryDelete = new StringBuilder(string.Format("DELETE FROM {0} ", tableName));
            queryDelete.Append(string.Format("WHERE {0} = {1}", propertyName, propertyValue));

            return queryDelete.ToString();
        }

        /// <summary>
        /// Build a multiple Delete  SQL Statement based on TX entity.
        /// </summary>
        /// <param name="enumerable">Enumerable entities.</param>
        /// <returns>Enumerable SQL Statement</returns>
        protected IEnumerable<string> BuilderOfDelete<TX>(IEnumerable<TX> enumerable)
        {
            return enumerable.Select(BuilderOfDelete).ToList();
        }

        /// <summary>
        /// Convert IEnumerable T of DataTable.
        /// </summary>
        /// <param name="enumerable">Enumerable entities to convert to DataRow.</param>
        /// <returns></returns>
        protected virtual DataTable ToDataTable<TX>(IEnumerable<TX> enumerable)
        {
            var dataTable = new DataTable();
            if (enumerable != null)
            {
                foreach (var item in enumerable)
                {
                    var itemType = item.GetType();
                    var properties = itemType.GetProperties();

                    if (dataTable.Columns.Count == 0)
                    {
                        foreach (var property in properties)
                        {
                            var ignore = Attribute.IsDefined(property, typeof(Ignore));
                            if (ignore) continue;

                            Type columnType;
                            if (property.PropertyType.Name.Contains("Null")) columnType = typeof(string);
                            else columnType = property.PropertyType;
                            dataTable.Columns.Add(property.Name, columnType);
                        }
                    }

                    var dataRow = dataTable.NewRow();
                    foreach (var property in properties)
                    {
                        var ignore = Attribute.IsDefined(property, typeof(Ignore));
                        if (ignore) continue;
                        var value = property.GetValue(item, null);
                        dataRow[property.Name] = value;
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }

        /// <summary>
        /// Convert IEnumerable T of DataTable.
        /// </summary>
        /// <param name="enumerable">Enumerable entities to convert to DataRow.</param>
        /// <returns></returns>
        protected virtual DataTable ToDataTable(IEnumerable<T> enumerable)
        {
            return ToDataTable<T>(enumerable);
        }

        protected void InsertBulk<TX>(IEnumerable<TX> items)
        {
            throw new NotImplementedException();
        }

        public void InsertBulk(IEnumerable<T> items)
        {
            InsertBulk(items);
        }
        #endregion
    }
}