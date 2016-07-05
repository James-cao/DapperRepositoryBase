using System;

namespace DapperRepositoryBase
{
    /// <summary>
    /// Attribute thata specified name of table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public  class TableName : System.Attribute
    {
        /// <summary>
        /// Name of table with her scheme.
        /// </summary>
        public string TableNameWithScheme { get; set; }


    }
}