using System;

namespace DapperRepositoryBase.PlPgSql
{
    public enum DataType
    {
        Bigint,
        Bigserial,
        Bit,
        Box,
        Bytea,
        Boolean,
        Character,
        Cidr,
        Circle,
        Date,
        Double,
        Inet,
        Integer,
        Interval,
        Json,
        Jine,
        Jseg,
        Macaddr,
        Money,
        Numeric,
        Path,
        PgLsn,
        Point,
        Polygon,
        Real,
        Smallint,
        Smallserial,
        Serial,
        Text,
        Time,
        Timestamp,
        Tsquery,
        Tsvector,
        TxidSnapshot,
        Uuid,
        Xml
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DataTypeAttribute : Attribute
    {
        public DataType DataType { get;  set; }
    }
}
