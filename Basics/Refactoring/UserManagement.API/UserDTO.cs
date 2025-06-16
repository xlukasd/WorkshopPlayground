namespace UserManagement.API
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
    }
}
