using System.Data;
using DapperRepositoryBase.Tests.Entities.PlPgSAql;
using DapperRepositoryBase.PlPgSql;


namespace DapperRepositoryBase.Tests.PlPgSql
{
    public interface IProductRepository : IRepositoryOperationBase<Product>
    {

    }
    public class ProductRepository :RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbConnection dbConnection)
            : base(dbConnection)
        {
        }
    }
}
