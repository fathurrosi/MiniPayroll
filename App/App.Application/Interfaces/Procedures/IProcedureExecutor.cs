
namespace App.Application.Interfaces.Procedures
{
    public interface IProcedureExecutor
    {
        Task<int> ExecuteNonQueryAsync(string storedProcedure, object? parameters = null);

        Task<T?> ExecuteScalarAsync<T>(string storedProcedure, object? parameters = null);

        Task<List<T>> ExecuteListAsync<T>(string storedProcedure, object? parameters = null);

        Task<T?> ExecuteSingleAsync<T>(string storedProcedure, object? parameters = null);
    }
}
