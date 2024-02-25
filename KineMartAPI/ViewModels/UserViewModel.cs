namespace KineMartAPI.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(string id, string userName, string email, string phoneNumber, bool lockoutEnable,
                                                                                           string role)
        {
            UserId = id;
            UserName = userName;
            Email = email;
            LockoutEnable = lockoutEnable;
            Role = role;
            PhoneNumber = phoneNumber;
        }

        public string UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool LockoutEnable { get; set; }
        public string Role { get; set; } = null!;
    }
}
