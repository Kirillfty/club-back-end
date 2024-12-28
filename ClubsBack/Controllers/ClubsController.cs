using ClubsBack.Entities;
using ClubsBack.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ClubsBack.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ClubsController  : ControllerBase
    {
        private IClubs<Clubs> _repository;
        public ClubsController(IClubs<Clubs> usersRepository)
        {
            _repository = usersRepository;
        }
        [HttpGet]
        public List<Clubs> Get() {
            return _repository.Get();
        }
    }
}
