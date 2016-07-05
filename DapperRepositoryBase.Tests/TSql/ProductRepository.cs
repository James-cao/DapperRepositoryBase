using System.Data;
using DapperRepositoryBase.Tests.Entities;
using DapperRepositoryBase.Tsql;

namespace DapperRepositoryBase.Tests.TSql
{
    public interface IProductRepository : IRepositoryOperationBase<Product>
    {

    }
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbConnection dbConnection)
            : base(dbConnection)
        {
        }
    }
}
