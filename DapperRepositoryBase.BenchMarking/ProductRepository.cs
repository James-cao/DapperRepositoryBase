using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperRepositoryBase.Tsql;

namespace DapperRepositoryBase.BenchMarking
{

    public interface IProductRepository : IRepositoryOperationBase<Product2>
    {

    }
    public class ProductRepository : RepositoryBase<Product2>, IProductRepository
    {
        public ProductRepository(IDbConnection dbConnection)
            : base(dbConnection)
        {
        }
    }
}
