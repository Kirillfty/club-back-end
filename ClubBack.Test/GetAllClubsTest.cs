using ClubsBack.Entities;
using ClubsBack.Repository;
using ClubsBack.Новая_папка;
using Moq;

namespace ClubBack.Test
{
    public class GetAllClubsTest
    {
        [Fact]
        public async Task ShouldReturnClubs()
        {
            Mock<IClubs<Clubs>> mock = new Mock<IClubs<Clubs>>();
            //hachalo
            mock.Setup(mock => mock.Get()).Returns(new List<Clubs> { new Clubs {id=1,title="ё",description="huy" } });

           var obj = new GetAllClubsCommandHandller(mock.Object);

            List<Clubs> list = await obj.Handle(new GetAllClubsCommand(),default);
            Assert.NotEmpty(list);
        }
    }
}