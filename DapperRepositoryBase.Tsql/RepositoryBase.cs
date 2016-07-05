using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;

namespace DapperRepositoryBase.Tsql
{
    /// <summary>
    /// Repository base is based on Micro-ORM Dapper, it has all the artifacts necessary to do CRUD on SQL SERVER.
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
        protected int CommandTimeout { get; set; }

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

        public virtual void InsertBulk(IEnumerable<T> items)
        {
            InsertBulk<T>(items);
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
            var result = DbConnection.ExecuteScalar<int>(query);
            return result;
        }

        protected void InsertBulk<TX>(IEnumerable<TX> items)
        {
            var sqlConnection = (SqlConnection)DbConnection;

            var bulkCopy = new SqlBulkCopy(sqlConnection);
            bulkCopy.DestinationTableName = GetTableName(typeof(TX));
            bulkCopy.BatchSize = 5000;
            var dataTable = ToDataTable(items);
            foreach (DataColumn column in dataTable.Columns)
            {
                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
            }
            if (sqlConnection.State == ConnectionState.Closed) sqlConnection.Open();
            bulkCopy.WriteToServer(dataTable);
        }

        #region -Const
        /// <summary>
        /// Format of date that is handled.
        /// </summary>
        private const string DataTimeFormat = "MM/dd/yyyy HH:mm:ss";
        /// <summary>
        /// SQL Statement that return the new Row.
        /// </summary>
        private const string ReturnRow = "OUTPUT INSERTED.* ";
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

        private string CreateValueQuery(PropertyInfo property, object propertyValue, bool comma)
        {
            if (propertyValue == null) return "null";
            if (!Attribute.IsDefined(property, typeof(DataTypeAttribute))) return string.Empty;

            var attribute = property.GetCustomAttribute<DataTypeAttribute>();
            var queryValues = new StringBuilder();
            Type typeCustom;
            IsNullable(property, out typeCustom);

            switch (attribute.DataType)
            {
                case DataType.Bigint:
                case DataType.Int:
                case DataType.SmallInt:
                case DataType.Tinyint:
                case DataType.Decimal:
                case DataType.Money:
                case DataType.SmallMoney:
                case DataType.Float:
                    queryValues.Append(string.Format("{0} ", propertyValue));
                    break;
                case DataType.Bit:
                    queryValues.Append(string.Format("{0} ", Convert.ToByte(propertyValue)));
                    break;
                case DataType.SmallDateTime:
                case DataType.DateTime:
                    if (typeCustom != typeof(DateTime)) break;
                    var smalldatetime = (DateTime)propertyValue;
                    queryValues.Append(string.Format("CONVERT({0}, '{1}',101) ", attribute.DataType, smalldatetime.ToString(DataTimeFormat)));
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
        /// <param name="returnId">Add select scope identity.</param>
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

                var comma = index > 0 ? "," : "";
                var property = properties[index];
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity, null);

                if (!Attribute.IsDefined(property, typeof(DataTypeAttribute))) continue;

                queryParameter.Append(string.Format("{1}{0} ", propertyName, comma));
                queryValues.Append(CreateValueQuery(property, propertyValue, index > 0));
            }

            var query = new StringBuilder(string.Format("INSERT INTO {0} ({1}) {3} VALUES ({2}) ", tableName, queryParameter, queryValues, returnObj ? ReturnRow : string.Empty));

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
        /// Build a Update SQL Statement based on TX entity.
        /// </summary>
        /// <param name="entity">Object to update.</param>
        /// <returns>SQL Statement.</returns>
        protected string BuilderOfUpdate<TX>(TX entity)
        {
            var itemType = entity.GetType();
            var properties = itemType.GetProperties();
            var tableName = GetTableName(itemType);
            var queryUpdate = new StringBuilder(string.Format("UPDATE {0} ", tableName));
            queryUpdate.Append("SET ");
            var condition = "WHERE ";

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity, null);


                if (Attribute.IsDefined(property, typeof(Identity)))
                {
                    condition += string.Format(" {0} = {1} ", propertyName, propertyValue);
                }
                if (Attribute.IsDefined(property, typeof(Ignore))) continue;
                if (!Attribute.IsDefined(property, typeof(DataTypeAttribute))) continue;

                queryUpdate.Append(string.Format("{0} = {1}, ", propertyName,
                    CreateValueQuery(property, propertyValue, false)));
            }
            queryUpdate.Append(ReturnRow);
            queryUpdate.Append(condition);

            var lastComme = queryUpdate.ToString().LastIndexOf(',');
            var result = queryUpdate.ToString().Remove(lastComme, 1);

            return result;
        }

        /// <summary>
        /// Build a multiple Update  SQL Statement based on TX entity.
        /// </summary>
        /// <typeparam name="TX">Entity Poco</typeparam>
        /// <param name="enumerable">Enumearble entities.</param>
        /// <returns>Enumerable SQL Statement</returns>
        protected IEnumerable<string> BuilderOfUpdate<TX>(IEnumerable<TX> enumerable)
        {
            return enumerable.Select(BuilderOfUpdate).ToList();
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
            if (enumerable == null) return dataTable;
            var prop = typeof(TX).GetProperties();
            //foreach (var property in prop)
            for (int index = 0; index < prop.Length; index++)
            {
                var property = prop[index];

                Type columnType;
                if (property.PropertyType.Name.Contains("Null")) columnType = typeof(string);
                else columnType = property.PropertyType;
                dataTable.Columns.Add(property.Name, columnType);
            }

            foreach (var item in enumerable)
            {
                var itemType = item.GetType();
                var properties = itemType.GetProperties();
                dataTable.Rows.Add(properties.Select(p => p.GetValue(item, null)).ToArray());
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


        #endregion
    }
}