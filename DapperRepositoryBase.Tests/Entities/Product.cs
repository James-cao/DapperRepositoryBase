using System;
using DapperRepositoryBase.Tsql;

namespace DapperRepositoryBase.Tests.Entities
{
    [TableName(TableNameWithScheme = "[dbo].[product]")]
    public class Product
    {
        [Identity, Ignore,DataType(DataType = DataType.Int)]
        public int Id { get; set; }
        [DataType(DataType = DataType.Varchar)]
        public string Name { get; set; }
        [Ignore,DataType(DataType = DataType.Varchar)]
        public string Description { get; set; }
        [DataType(DataType = DataType.Bit)]
        public bool IsEnable { get; set; }
        [DataType(DataType = DataType.DateTime)]
        public DateTime LastUpdate { get; set; }
        [DataType(DataType = DataType.Decimal)]
        public Decimal Prices { get; set; }

    }
}
