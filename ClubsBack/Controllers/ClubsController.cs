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
        public ActionResult Post([FromBody] Clubs user)
        {
            if (_repository.CreateClub(user) == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete([FromRoute] int id)
        {
            if (_repository.Delete(id) == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
