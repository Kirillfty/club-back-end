using ClubsBack.Entities;
using Microsoft.Data.Sqlite;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace ClubsBack.Repository
{
    public class UsersRepository : IRepository<Users>
    {
        private readonly DBconnect _options;
        public UsersRepository(DBconnect options)
        {
            _options = options;
        }

        public bool Delete(int id)
        {
            using (SqliteConnection conn = new SqliteConnection(_options.Connect)) {
                int result = conn.Execute("DELETE * FROM Users WHERE id = @id",new {id = id });
                if (result != 0)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        public List<Users> Get() {
            using (SqliteConnection conn = new SqliteConnection(_options.Connect)) {
                return conn.Query<Users>("SELECT * FROM Users").ToList();
            }
        }

        public Users? GetById(int id)
        {
            using (SqliteConnection conn = new SqliteConnection(_options.Connect))
            {
                return conn.QueryFirstOrDefault<Users>("SELECT * FROM Users WHERE id = @id", new { id = id });
            }
        }

        public int? Insert(Users item)
        {
            using (SqliteConnection conn = new SqliteConnection(_options.Connect))
            {
                int result = conn.Execute("INSERT INTO Users (firstName, lastName, nickName ,password,role) VALUES (@firstName, @lastName, @nickName,@password,@Role)", item);
                if (result != 0)
                {
                   int id = conn.QuerySingle<int>("SELECT last_insert_rowid();");
                    
                    return id;
                }
                else {
                    return null;
                }
            
            }
        }

        public bool Update(Users item)
        {
            using (SqliteConnection conn = new SqliteConnection(_options.Connect))
            {
                int result = conn.Execute("UPDATE Users SET firstName = @firstName, lastName = @lastName, nickName = @nickName WHERE id = @id", item);

                if (result != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool SetRefreshToken(string refreshToken, string nickName)
        {
            using (var connection = new SqliteConnection(_options.Connect))
            {
                var result = connection.Execute($"UPDATE Users SET RefreshToken = @refreshToken where NickName = @nickName", new { nickName, refreshToken });
                return result > 0;
            }
        }

        
    }
}
