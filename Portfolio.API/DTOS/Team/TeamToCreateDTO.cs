namespace Portfolio.API.DTOS.Team
{
    public class TeamToCreateDTO
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public IFormFile PictureUrl { get; set; }
    }
}
