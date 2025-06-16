namespace UserManagement.API.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
    }
}
