namespace UsersRESTApi.Controllers
{
    using System.Collections.Generic;

    using Lib.Users.Domain;
    using Lib.Users.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: users
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_userRepository.GetUsers().Results);
        }

        // POST users
        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            _userRepository.InsertUser(user);

            return Ok();
        }
    }
}