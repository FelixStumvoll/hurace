using System;
using System.Data.Common;

namespace Hurace.Dal.Common
{
    public static class DbUtil
    {
        public static DbProviderFactory GetProviderFactory(string providerName) =>
            providerName switch
            {
                "Microsoft.Data.SqlClient" => (DbProviderFactory) Microsoft.Data.SqlClient.SqlClientFactory.Instance,
                "MySql.Data.MySqlClient" => MySql.Data.MySqlClient.MySqlClientFactory.Instance,
                _ => throw new ArgumentException($"Invalid provider name \"{providerName}\"")
            };
    }
}