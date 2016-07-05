using System.Configuration;
using System.Linq;
using System.Transactions;
using DapperRepositoryBase.Tests.Entities.PlPgSAql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using Ploeh.AutoFixture;

namespace DapperRepositoryBase.Tests.PlPgSql
{
    [TestClass]
    public class RepositoryBaseTest
    {
        [TestMethod]
        public void Get_Success()
        {
            using (var trans = new TransactionScope())
            {
                var fixture = new Fixture();
                var item = fixture.Create<Product>();

                var repository = GetRepository();
                var results = repository.Get();

                Assert.IsTrue(results.Any(), "Ups, Está vacio.");
            }
        }

        [TestMethod]
        public void Add_ReturnId_Success()
        {
            using (var trans = new TransactionScope())
            {

                var fixture = new Fixture();
                var item = fixture.Create<Product>();
                var repository = GetRepository();
                
                var newItem = repository.Add(item);

                Assert.IsNotNull(newItem);

            }
        }
        [TestMethod]
        public void Update_IgnoreProperty_ReturnNull()
        {
            using (var trans = new TransactionScope())
            {
                var fixture = new Fixture();
                //var item = fixture.Create<Product>();
                var repository = GetRepository();
                var item = repository.Get().FirstOrDefault();
                var change = fixture.Create<string>();

                item.Description = change;
                repository.Update(item);

                var result = repository.Get().FirstOrDefault();

                Assert.IsNull(result.Description);
            }
        }
        [TestMethod]
        public void Update_Success()
        {
            using (var trans = new TransactionScope())
            {
                var fixture = new Fixture();
                //var item = fixture.Create<Product>();
                var repository = GetRepository();
                var item = repository.Get().FirstOrDefault();
                var change = fixture.Create<string>();

                item.Name = change;
                var result = repository.Update(item);

                Assert.AreEqual(change, result.Name);
            }
        }


        public IProductRepository GetRepository()
        {
            var conectionString = ConfigurationManager.ConnectionStrings["DbConnectionPostgre"].ConnectionString;
            return new ProductRepository(new NpgsqlConnection(conectionString));
        }
    }

}
