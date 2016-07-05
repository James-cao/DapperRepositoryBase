using System;

using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DapperRepositoryBase.Tsql;
using EntityFramework.BulkInsert.Extensions;
using Ploeh.AutoFixture;
using DapperRepositoryBase.BenchMarking.data;
namespace DapperRepositoryBase.BenchMarking
{
    [TableName(TableNameWithScheme = "[dbo].[product]")]
    public class Product2
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
    class Program
    {
        static void Main(string[] args)
        {
            DapperBench();
           // Ef6WithBulk();
          Console.ReadKey();
        }

        static void Ef6WithBulk()
        {

            var fixture = new Fixture();

            var item = fixture.CreateMany<Product>(10000);

            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Iniciando comparación");
            Console.WriteLine("==ENTITY FRAMEWORK 6 WITH BULK==");
            Console.WriteLine("Insertando {0} registros", item.Count());

            using (var transactionScope = new TransactionScope())
            {
                var ctx = new CatalogEntities();

                stopwatch.Start();
                ctx.BulkInsert(item);
                ctx.SaveChanges();
                stopwatch.Stop();

                Console.WriteLine("Duracion {0} ms ", stopwatch.ElapsedMilliseconds);
                Console.WriteLine("Duracion {0} se ", stopwatch.Elapsed.Seconds);
                Console.WriteLine("Duracion {0} mi ", stopwatch.Elapsed.Minutes);
                stopwatch.Reset();

                Console.WriteLine("Obteniendo TODO los registros");
                stopwatch.Start();
                var reponse = ctx.Product;
                stopwatch.Stop();
                Console.WriteLine("Se obtuvo {0} en una duracion de {1} ms", reponse.Count(), stopwatch.ElapsedMilliseconds);
                Console.WriteLine("Se obtuvo {0} en una duracion de {1} se", reponse.Count(), stopwatch.Elapsed.Seconds);
                Console.WriteLine("Se obtuvo {0} en una duracion de {1} mi", reponse.Count(), stopwatch.Elapsed.Minutes);
            }

         
        }
        static void Ef6()
        {
            var fixture = new Fixture();

            var item = fixture.CreateMany<Product>(10000);

            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Iniciando comparación");
            Console.WriteLine("==ENTITY FRAMEWORK 6==");
            Console.WriteLine("Insertando {0} registros", item.Count());
            using (var transactionScope = new TransactionScope())
            {
                CatalogEntities ctx = new CatalogEntities();

                stopwatch.Start();
                ctx.Product.AddRange(item);
                ctx.SaveChanges();
                stopwatch.Stop();

                Console.WriteLine("Duracion {0} ms ", stopwatch.ElapsedMilliseconds);
                Console.WriteLine("Duracion {0} se ", stopwatch.Elapsed.Seconds);
                Console.WriteLine("Duracion {0} mi ", stopwatch.Elapsed.Minutes);
                stopwatch.Reset();

                Console.WriteLine("Obteniendo TODO los registros");
                stopwatch.Start();
                var reponse = ctx.Product;
                stopwatch.Stop();
                Console.WriteLine("Se obtuvo {0} en una duracion de {1} ms", reponse.Count(),
                    stopwatch.ElapsedMilliseconds);
                Console.WriteLine("Se obtuvo {0} en una duracion de {1} se", reponse.Count(), stopwatch.Elapsed.Seconds);
                Console.WriteLine("Se obtuvo {0} en una duracion de {1} mi", reponse.Count(), stopwatch.Elapsed.Minutes);

            }
           
        }
        static void DapperBench()
        {
            var fixture = new Fixture();
            var item = fixture.CreateMany<Product2>(10000);
            var conectionString = ConfigurationManager.ConnectionStrings["DbConnectionSql"].ConnectionString;

            var repo = new ProductRepository(new SqlConnection(conectionString));
            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Iniciando comparación");
            Console.WriteLine("==DAPPER REPOSITORY==");
            Console.WriteLine("Insertando {0} registros", item.Count());
            using (var transactionScope = new TransactionScope())
            {
                stopwatch.Start();
                repo.Add(item);
                stopwatch.Stop();
                Console.WriteLine("Duracion {0} ms ", stopwatch.ElapsedMilliseconds);
                Console.WriteLine("Duracion {0} se ", stopwatch.Elapsed.Seconds);
                Console.WriteLine("Duracion {0} mi ", stopwatch.Elapsed.Minutes);
                stopwatch.Reset();

                Console.WriteLine("Obteniendo TODO los registros");
                stopwatch.Start();
                var reponse = repo.Get();
                stopwatch.Stop();
                Console.WriteLine("Se obtuvo {0} en una duracion de {1} ms", reponse.Count(),
                    stopwatch.ElapsedMilliseconds);
                Console.WriteLine("Se obtuvo {0} en una duracion de {1} se", reponse.Count(), stopwatch.Elapsed.Seconds);
                Console.WriteLine("Se obtuvo {0} en una duracion de {1} mi", reponse.Count(), stopwatch.Elapsed.Minutes);

            }
            
        }
    }
}
