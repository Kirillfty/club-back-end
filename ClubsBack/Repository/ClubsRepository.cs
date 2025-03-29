using ClubsBack.Entities;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ClubsBack.Repository
{
    public class ClubsRepository : IClubs
    {
        private readonly DBconnect _options;
        public ClubsRepository(DBconnect options)
        {
            _options = options;
        }
        public bool CreateClub(Clubs item)
        {
            using (SqliteConnection conn = new SqliteConnection(_options.Connect))
            {
                int result = conn.Execute("INSERT INTO Clubs (title, description) VALUES (@title, @description)", item);
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
        public bool Delete(int id)
        {
            using (SqliteConnection conn = new SqliteConnection(_options.Connect))
            {
                int result = conn.Execute("DELETE FROM Clubs WHERE id = @id",new { id = id });
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
        public bool CheckUserOwnClub(ClubsUsers item){
            using (SqliteConnection conn = new SqliteConnection(_options.Connect)) { 
                ClubsUsers? result = conn.QueryFirstOrDefault<ClubsUsers>($"SELECT * FROM ClubsUsers WHERE clubId = @clubId AND userId = @userId AND isAdmin = @isAdmin",item);
                if(result != null){
                    return true;
                }
                else{
                    return false;
                }
                
            }
        }
        public bool ExitClub(int id)
        {
            using (SqliteConnection conn = new SqliteConnection(_options.Connect)) { 
                int result =  conn.Execute(@"DELETE FROM ClubUsers WHERE Id = @Id", new {Id = id});
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

        public List<Clubs> Get()
        {
            using (SqliteConnection conn = new SqliteConnection(_options.Connect)) { 
                 return conn.Query<Clubs>("SELECT * FROM Clubs").ToList();
            }
        }

        public Clubs? GetById(int id)
        {
            using (SqliteConnection conn = new SqliteConnection(_options.Connect))
            {
                return conn.QueryFirstOrDefault<Clubs>("SELECT * FROM Clubs WHERE id = @id", new { Id = id });
            }
        }

        public bool SignClub(int clubId, int userId)
        {
            using (SqliteConnection conn = new SqliteConnection(_options.Connect))
            {
                int result = conn.Execute("INSERT INTO ClubsUsers (clubId, userId) VALUES (@clubId, @userId)", new { clubId, userId });

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

        public bool Update(Clubs item)
        {
            using (SqliteConnection conn = new SqliteConnection(_options.Connect))
            {
                int result = conn.Execute("UPDATE Clubs SET title = @title, description = @description WHERE id = @id", item);

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
    }
}
