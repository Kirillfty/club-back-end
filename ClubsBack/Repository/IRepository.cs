namespace ClubsBack.Repository
{
    public interface IRepository<Item>
    {
        public bool Post(Item item);

        //удаление
        public bool Delete(int id);

        //редактирование
        public bool Put(Item item);

        //получение одного элемента
        public Item? GetById(int id);

        //получение всех
        public List<Item> Get();
        bool SetRefreshToken(string refreshToken, string nickName);
    }
}
