using BankingApi.Data;
using BankingApi.Models;
using BankingApi.Repositories.Interfaces;
using Dapper;

namespace BankingApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = """
            SELECT
                Id,
                FullName,
                Email,
                PasswordHash,
                CreatedAt
            FROM Users
            WHERE Email = @Email;
            """;

        using var connection = _context.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<User>(
            sql,
            new { Email = email });
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                Id,
                FullName,
                Email,
                PasswordHash,
                CreatedAt
            FROM Users
            WHERE Id = @Id;
            """;

        using var connection = _context.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<User>(
            sql,
            new { Id = id });
    }

    public async Task<int> CreateAsync(User user)
    {
        const string sql = """
            INSERT INTO Users
            (
                FullName,
                Email,
                PasswordHash
            )
            VALUES
            (
                @FullName,
                @Email,
                @PasswordHash
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        using var connection = _context.CreateConnection();

        return await connection.QuerySingleAsync<int>(
            sql,
            new
            {
                user.FullName,
                user.Email,
                user.PasswordHash
            });
    }
}