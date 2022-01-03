using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Common.Validation;
using TaskManager.Common.Validation.ValidationModel;
using TaskManager.Database;
using TaskManager.Models.Domain.User;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class UsersController : Controller
    {
        private readonly UserRepository _repo;

        public UsersController(TaskManagerContext context)
        {
            _repo = new UserRepository(context);
        }

        [HttpPost("create")]
        public async Task<ActionResult> PostCreate([FromBody]UserCreate user)
        {
            var existingUser = await _repo.GetUser(user.Email);

            if (existingUser != null)
            {
                return new ValidationResult(
                    "Duplicate email error",
                    400,
                    new List<ValidationError> { new ($"A user with email {user.Email} already exists.") }
                );
            }

            var createdUser = user.ToCreatedUser();

            await _repo.AddUser(createdUser);

            return new ValidationResult($"User {user.FirstName} {user.LastName} created.", 200);
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var result = await _repo.GetUsers();

            var users = result.Select(x => x.ToQueryObject()).ToArray();

            return new JsonResult(users);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult> GetUser(string email)
        {
            var user = await _repo.GetUser(email);

            if (user == null)
            {
                return new ValidationResult(
                    "Invalid email",
                    404,
                    new List<ValidationError> { new ($"A user with email {email} does not exist.") }
                );
            }

            return new JsonResult(user.ToQueryObject());
        }
    }
}