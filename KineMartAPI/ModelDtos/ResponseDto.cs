using KineMartAPI.ViewModels;

namespace KineMartAPI.ModelDtos
{
    public class ResponseDto
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime ExpiredAt { get; set; }
        public UserViewModel User { get; set; } = null!;
    }
}
