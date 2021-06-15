using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.ORM.Dapper.Interfaces
{
    public interface IDapperService
    {
        /// <summary>
        /// SQLs the query asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> SqlQueryAsync<T>(string sql, DynamicParameters parameters) where T : class;

        /// <summary>
        /// SQLs the execute asynchronous.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        Task<int> SqlExecuteAsync(string sql, DynamicParameters parameters = null);

        /// <summary>
        /// Gets the data distinct by identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlFileName">Name of the SQL file.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetDataDistinctById<T>(string sqlFileName, DynamicParameters parameters) where T : class;
    }
}
