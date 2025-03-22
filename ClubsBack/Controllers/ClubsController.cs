using ClubsBack.Entities;
using ClubsBack.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubsBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        [HttpPost]
        [Route("create")]
        [Authorize]
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
        public record ClubId(int clubId,int userId);
        [HttpPost]
        public ActionResult Signclub([FromBody]  ClubId clubId)
        {
            if (_repository.SignClub(clubId.clubId,clubId.userId) == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("exit-club/{id:int}")]
       
        public ActionResult ExitClubs([FromRoute] int id)
        {
            if (_repository.ExitClub(id) == true)
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
