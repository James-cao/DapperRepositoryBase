using System;

namespace DapperRepositoryBase
{
    /// <summary>
    /// Ignore the property to get or insert process database
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property)]
    public class Ignore : System.Attribute
    {
         
    }
}