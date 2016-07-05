using System;

namespace DapperRepositoryBase.Tsql
{
    public enum DataType
    {
        Bigint,
        Bit,
        Decimal,
        Int,
        Money,
        Numeric,
        SmallInt,
        SmallMoney,
        Tinyint,
        Float,
        Real,
        Date,
        DateTime2,
        DateTime,
        DateTimeOffSet,
        SmallDateTime,
        Time,
        Char,
        Text,
        Varchar,
        Nchar,
        NText,
        Nvarchar,
        Binary,
        Imagen,
        VarBinary,
        TimeStamp,
        Xml
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DataTypeAttribute : Attribute
    {

        public DataType DataType { get; set; }

    }
}