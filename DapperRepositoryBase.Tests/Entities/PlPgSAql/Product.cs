using System;
using DapperRepositoryBase.PlPgSql;

namespace DapperRepositoryBase.Tests.Entities.PlPgSAql
{
    [TableName(TableNameWithScheme = "public.product")]
    public class Product
    {
        [Identity, Ignore, DataType(DataType = DataType.Serial)]
        public int Id { get; set; }
        [DataType(DataType = DataType.Character)]
        public string Name { get; set; }
        [DataType(DataType = DataType.Character), Ignore]
        public string Description { get; set; }
        [DataTypeAttribute(DataType = DataType.Boolean)]
        public bool IsEnable { get; set; }

        [DataTypeAttribute(DataType = DataType.Timestamp)]
        public DateTime? LastUpdate { get; set; }
        [DataTypeAttribute(DataType = DataType.Double)]
        public Decimal Prices { get; set; }
    }
}
