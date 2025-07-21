using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Agri_EnergyConnect.Models;

namespace Agri_EnergyConnect.Models
{
    public class DatabaseHelper
    {
        private static readonly Lazy<DatabaseHelper> _instance = new(() => new DatabaseHelper());
        private readonly string _connectionString;

        private DatabaseHelper()
        {
            _connectionString = "Server=tcp:eventease6.database.windows.net,1433;Initial Catalog=AgriEnergyConnect;Persist Security Info=False;User ID=eventEase;Password=GreatCopy@2024;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        public static DatabaseHelper Instance => _instance.Value;

       
        public async Task AddUser(AppUser user)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            string query = @"
                INSERT INTO AppUsers (FullName, Email, PhoneNumber, Password, Role)
                VALUES (@FullName, @Email, @PhoneNumber, @Password, @Role)"; 

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@FullName", user.FullName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
            cmd.Parameters.AddWithValue("@Password", user.Password); 
            cmd.Parameters.AddWithValue("@Role", user.Role); 

            await cmd.ExecuteNonQueryAsync();
        }

        
        public async Task<bool> ValidateUser(string email, string password)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            string query = @"SELECT COUNT(*) FROM AppUsers WHERE Email = @Email AND Password = @Password";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);

            var result = (int)await cmd.ExecuteScalarAsync();
            return result > 0;
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            string query = @"SELECT FullName, Email, PhoneNumber, Password, Role FROM AppUsers WHERE Email = @Email";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new AppUser
                {
                    FullName = reader.GetString(0),
                    Email = reader.GetString(1),
                    PhoneNumber = reader.GetString(2),
                    Password = reader.GetString(3),
                    Role = reader.GetString(4)
                };
            }

            return null; 
        }
    }
}
