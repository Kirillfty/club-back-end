using ClubsBack.Entities;
using ClubsBack.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubsBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClubsController : ControllerBase
    {
        private IClubs _repository;
        public ClubsController(IClubs usersRepository)
        {
            _repository = usersRepository;
        }
        [HttpGet]
        public List<Clubs> Get()
        {
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
        [Authorize]
        public ActionResult Delete([FromRoute] int id)
        {
            var user = HttpContext.User.Identity.Name;
            ClubsUsers clubUsers = new ClubsUsers { userId = int.Parse(user), clubId = id, isAdmin = 1 };
            if (_repository.CheckUserOwnClub(clubUsers))
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
            else{
                return BadRequest("не прошел проверку на пользователя");
            }


        }
        public record ClubId(int clubId, int userId);
        [HttpPost]
        public ActionResult Signclub([FromBody] ClubId clubId)
        {
            if (_repository.SignClub(clubId.clubId, clubId.userId) == true)
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
