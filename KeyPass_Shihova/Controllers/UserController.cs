using KeyPass_Shihova.Classes;
using KeyPass_Shihova.Models;
using Microsoft.AspNetCore.Mvc;

namespace KeyPass_Shihova.Controllers
{
    [Route("/user")]
    public class UserController : Controller
    {
        private DatabaseManager databaseManager;
        public UserController()
        {
            this.databaseManager = this.databaseManager = new DatabaseManager();
        }
        [Route("login")]
        [HttpPost]
        public ActionResult Login([FromForm] string login, [FromForm] string password) 
        {
            try
            {
                User? AuthUser = databaseManager.Users.Where(x => x.Login == login && x.Password == password).FirstOrDefault();
                if (AuthUser == null)
                {
                    return StatusCode(401);
                }
                else 
                {
                    string Token = JwtToken.Generate(AuthUser);
                    AuthUser.LastAuth = DateTime.Now;
                    databaseManager.SaveChanges();
                    return Ok(new { token = Token });
                }
            }
            catch (Exception ex) 
            {
                return StatusCode(501, ex.Message);
            }
        }
    }
}
