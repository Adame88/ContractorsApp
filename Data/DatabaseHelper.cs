using System.Data;
using System.Data.SqlClient;

namespace ContractorsApp.Data
{
    internal static class DatabaseHelper
    {
        internal static async Task<T> ExecuteWithTransactionAsync<T>(Func<SqlTransaction, Task<T>> operation)
        {
            using (var conn = DatabaseFactory.CreateConnection())
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var result = await operation(transaction);
                        await transaction.CommitAsync();
                        return result;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }
    }

}
