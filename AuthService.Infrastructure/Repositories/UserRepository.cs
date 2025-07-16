using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Database;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;

namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM User WHERE Email = @Email";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task AddUserAsync(User user)
        {
            user.Id = Guid.NewGuid(); // Garante que o GUID foi atribuído

            var sql = @"INSERT INTO User 
        (Id, Name, Email, Cpf, PasswordHash, Role, Position, CreatedAt)
        VALUES 
        (@Id, @Name, @Email, @Cpf, @PasswordHash, @Role, @Position, @CreatedAt)";

            using var connection = _context.CreateConnection();

            try
            {
                await connection.ExecuteAsync(sql, user);
            }
            catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
            {
                if (ex.Message.Contains("user.Email"))
                    throw new ApplicationException("Já existe um usuário com esse e-mail.");
                if (ex.Message.Contains("user.Cpf"))
                    throw new ApplicationException("Já existe um usuário com esse CPF.");

                throw; // rethrow se não for e-mail nem CPF
            }
        }


    }
}
