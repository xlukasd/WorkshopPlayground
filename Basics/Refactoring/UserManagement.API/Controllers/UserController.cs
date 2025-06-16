using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly UserMapper _userMapper;

        public UserController()
        {
            _userRepository = new UserRepository();
            _userMapper = new UserMapper();
        }

        [HttpGet(Name = "GetUsers")]
        public IEnumerable<UserDTO> Get()
        {
            IReadOnlyCollection<Model.User> users = _userRepository.GetUsers();
            return users.Select(_userMapper.Map);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(Guid id)
        {
            Model.User user = _userRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(_userMapper.Map(user));
        }

        [HttpPost(Name = "CreateUser")]
        public IActionResult Post([FromBody] UserDTO userDTO)
        {
            Model.User user = _userMapper.Map(Guid.NewGuid(), userDTO);
            _userRepository.CreateUser(user);
            return CreatedAtRoute("GetUser", user.Id);
        }

        [HttpPut("{id}", Name = "UpdateUser")]
        public IActionResult Put(Guid id, [FromBody] UserDTO userDTO)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            _userMapper.Map(user, userDTO);

            _userRepository.UpdateUser(user);
            return NoContent();
        }
    }
}
