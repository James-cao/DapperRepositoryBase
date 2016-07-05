using Ploeh.AutoFixture;

namespace DapperRepositoryBase.Tests
{
   static class CompositionRoot
    {
        static IFixture FixtureInstance
        {
            get { var fix = new Fixture();
                return fix;
            }
        }
       
    }
}
