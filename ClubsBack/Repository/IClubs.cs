namespace ClubsBack.Repository
{
    public interface IClubs<Item>
    {
        public Item? GetById(int id);

        public List<Item> Get();

        public bool CreateClub(Item item);

        public bool Update(Item item);

        public Item? SignClub(int id);

        public bool ExitClub(int id);
        public bool Delete(int id);
    }
}
