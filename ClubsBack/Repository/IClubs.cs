using ClubsBack.Entities;
namespace ClubsBack.Repository
{
    public interface IClubs
    {
        public Clubs? GetById(int id);

        public List<Clubs> Get();

        public bool CreateClub(Clubs item);

        public bool Update(Clubs item);

        public bool SignClub(int clubId,int userId);
        bool CheckUserOwnClub(ClubsUsers item);

        public bool ExitClub(int id);
        public bool Delete(int id);
    }
}
