namespace Portfolio.API.DTOS.Users
{
    public class UserResponseDTO
    {
        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string? Role { get; set; }  

        public string Token { get; set; }

    }
}
