using ClubsBack.Entities;
using ClubsBack.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ClubsBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IRepository<Users> _repository;
        public UsersController(IRepository<Users> usersRepository)
        {
            _repository = usersRepository;
        }

        [HttpGet]
        public List<Users> Get()
        {
            return _repository.Get();
        }
        [HttpPost]
        public ActionResult Post([FromBody] Users user)
        {
            if (_repository.Insert(user) != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult GetById([FromRoute] int id)
        {
            Users? result = _repository.GetById(id);

            if (result == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete([FromBody] int id)
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
        [HttpPut]
        public ActionResult Update([FromBody] Users user)
        {
            if (_repository.Update(user) == true)
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
