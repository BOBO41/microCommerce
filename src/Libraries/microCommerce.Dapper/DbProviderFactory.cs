using microCommerce.Common;
using microCommerce.Dapper.Providers;
using microCommerce.Dapper.Providers.MySql;
using microCommerce.Dapper.Providers.PostgreSql;
using microCommerce.Dapper.Providers.SqlServer;

namespace microCommerce.Dapper
{
    public class DbProviderFactory : IDbProviderFactory
    {
        public virtual IProvider Create(string providerName)
        {
            Check.IsEmpty(providerName);

            providerName = providerName.Trim().ToLower();
            switch (providerName)
            {
                case "system.data.sqlclient":
                    {
                        return new SqlServerProvider();
                    }
                case "mysql.data.sqlclient":
                    {
                        return new MySqlProvider();
                    }
                case "npsql.data.sqlclient":
                    {
                        return new PostgreSqlProvider();
                    }
                default:
                    return null;
            }
        }
    }
}