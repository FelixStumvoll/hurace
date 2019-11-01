using System.Diagnostics.CodeAnalysis;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao.QueryBuilder;

namespace Hurace.Core.Test
{
    [ExcludeFromCodeCoverage]
    public class TestBase
    {
        private const string ConnectionString =
            "Data Source=localhost;Initial Catalog=huraceDB;Persist Security Info=True;User ID=SA;Password=EHq(iT|$@A4q";

        private const string ProviderName = "Microsoft.Data.SqlClient";
        protected StatementFactory StatementFactory { get; } = new StatementFactory("hurace");
        protected ConcreteConnectionFactory ConnectionFactory { get; }

        protected TestBase()
        {
            ConnectionFactory =
                new ConcreteConnectionFactory(DbUtil.GetProviderFactory(ProviderName), ConnectionString, ProviderName);
        }
    }
}