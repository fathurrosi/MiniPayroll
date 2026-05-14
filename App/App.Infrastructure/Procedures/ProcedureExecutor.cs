using App.Application.Interfaces.Procedures;
using App.Infrastructure.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace App.Infrastructure.Procedures
{
    public class ProcedureExecutor : IProcedureExecutor
    {
        private readonly AppDBContext _context;

        public ProcedureExecutor(AppDBContext context)
        {
            _context = context;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_context.Database.GetConnectionString());
        }

        private int GetCommandTimeout()
        {
            int timeout = _context.Database.GetCommandTimeout() ?? 30;
            return timeout;
        } 
        public async Task<int> ExecuteNonQueryAsync(
            string storedProcedure,
            object? parameters = null)
        {
            using var connection = CreateConnection();

            return await connection.ExecuteAsync(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: GetCommandTimeout()
            );
        }
         
        public async Task<T?> ExecuteScalarAsync<T>(
            string storedProcedure,
            object? parameters = null)
        {
            using var connection = CreateConnection();

            return await connection.ExecuteScalarAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: GetCommandTimeout()
            );
        }
         
        public async Task<List<T>> ExecuteListAsync<T>(
            string storedProcedure,
            object? parameters = null)
        {
            using var connection = CreateConnection();

            var result = await connection.QueryAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: GetCommandTimeout()
            );

            return result.ToList();
        }
         
        public async Task<T?> ExecuteSingleAsync<T>(
            string storedProcedure,
            object? parameters = null)
        {
            using var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: GetCommandTimeout()
            );
        }
    }
}