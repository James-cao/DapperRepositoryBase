using DapperRepositoryBase;
using DapperRepositoryBase.Tsql;
using Ploeh.AutoFixture;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Transactions;
using System.Diagnostics;

namespace DapperRepository.BenchMarking
{
    [TableName(TableNameWithScheme = "[dbo].[product]")]
    public class Product
    {
        [Identity, Ignore, DataType(DataType = DataType.Int)]
        public int Id { get; set; }
        [DataType(DataType = DataType.Varchar)]
        public string Name { get; set; }
        [Ignore, DataType(DataType = DataType.Varchar)]
        public string Description { get; set; }
        [DataType(DataType = DataType.Bit)]
        public bool IsEnable { get; set; }
        [DataType(DataType = DataType.DateTime)]
        public DateTime LastUpdate { get; set; }
        [DataType(DataType = DataType.Decimal)]
        public Decimal Prices { get; set; }

    }
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

    class Program
    {
        static void Main(string[] args)
        {
            DapperRepository();
            Console.ReadKey();
        }

        static void DapperRepository()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("== DAPPER REPOSITORY ==");
            Console.ForegroundColor = ConsoleColor.Gray;
            var fixture = new Fixture();
            var items = fixture.CreateMany<Product>(10000);
            var conectionString = ConfigurationManager.ConnectionStrings["DbConnectionSql"].ConnectionString;
            using (var trans = new TransactionScope())
            {
                var repo = new ProductRepository(new SqlConnection(conectionString));
                var stopWatch = new Stopwatch();
                Console.WriteLine("== Insert Bulk ==");
                Console.WriteLine("Registros a insertar {0}", items.Count());
                stopWatch.Start();
                repo.InsertBulk(items);
                stopWatch.Stop();
                Console.WriteLine("Duracción {0} ", stopWatch.Elapsed);
                stopWatch.Reset();
                Console.WriteLine("== Get ==");
                stopWatch.Start();
                var result = repo.Get();
                stopWatch.Stop();
                Console.WriteLine("Obtuvo {0} y tardo {1} ", result.Count(), stopWatch.Elapsed);
            }

            Console.WriteLine();
            Console.WriteLine("===================================");
            using (var trans = new TransactionScope())
            {
                var repo = new ProductRepository(new SqlConnection(conectionString));
                var stopWatch = new Stopwatch();
                Console.WriteLine("== Add (Query based) ==");
                Console.WriteLine("Registros a insertar {0}", items.Count());
                stopWatch.Start();
                repo.Add(items);
                stopWatch.Stop();
                Console.WriteLine("Duracción {0} ", stopWatch.Elapsed);

                stopWatch.Reset();

                Console.WriteLine("== Get ==");
                stopWatch.Start();
                var result = repo.Get();
                stopWatch.Stop();
                Console.WriteLine("Obtuvo {0} y tardo {1} ", result.Count(), stopWatch.Elapsed);
            }

            Console.WriteLine();
            Console.WriteLine("===================================");
            using (var trans = new TransactionScope())
            {
                var repo = new ProductRepository(new SqlConnection(conectionString));
                var stopWatch = new Stopwatch();
                Console.WriteLine("== GET (Query based) ==");
                stopWatch.Start();
                var result = repo.Get();
                stopWatch.Stop();
                Console.WriteLine("Obtuvo {0} y tardo {1} ", result.Count(), stopWatch.Elapsed);
            }
        }
    }
}
