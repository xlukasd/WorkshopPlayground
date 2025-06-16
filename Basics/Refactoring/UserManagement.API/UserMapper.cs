using UserManagement.API.Model;

namespace UserManagement.API
{
    public class UserMapper
    {
        public UserDTO Map(User user)
        {
            return new UserDTO
            {
                UserName = user.UserName,
                HashedPassword = user.HashedPassword,
                Salt = user.Salt
            };
        }

        public User Map(Guid id, UserDTO userDTO)
        {
            return new User
            {
                Id = id,
                UserName = userDTO.UserName,
                HashedPassword = userDTO.HashedPassword,
                Salt = userDTO.Salt
            };
        }

        public void Map(User user, UserDTO userDTO)
        {
            user.UserName = userDTO.UserName;
            user.HashedPassword = userDTO.HashedPassword;
            user.Salt = userDTO.Salt;
        }
    }
}
