using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Database;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;


namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(DapperContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM auth_db.users WHERE Email = @Email";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task AddUserAsync(User user)
        {
            const string sql = @"
        INSERT INTO auth_db.users 
        (name, email, cpf, password_hash, role_id) 
        VALUES (@Name, @Email, @Cpf, @PasswordHash, @RoleId)";

            using var connection = _context.CreateConnection();

            try
            {
                await connection.ExecuteAsync(sql, new
                {
                    Name = user.Name,
                    Email = user.Email,
                    Cpf = user.Cpf,
                    PasswordHash = user.Password_Hash,
                    RoleId = user.Role_Id
                });
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                if (ex.Message.Contains("email"))
                    throw new ApplicationException("Já existe um usuário com esse e-mail.");
                if (ex.Message.Contains("cpf"))
                    throw new ApplicationException("Já existe um usuário com esse CPF.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao adicionar usuário: {Email}, {Cpf}", user.Email, user.Cpf);
                throw new ApplicationException("Erro inesperado ao salvar o usuário.");
            }
        }

        public async Task<bool> DeleteUserAsync(ulong id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM auth_db.users WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

            return rowsAffected > 0;
        }

        public async Task<User?> GetByIdAsync(ulong id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM auth_db.users WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }
    }
}
