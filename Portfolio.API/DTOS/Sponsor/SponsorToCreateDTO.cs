namespace Portfolio.API.DTOS.Sponsor
{
    public class SponsorToCreateDTO
    {
        public string? Name { get; set; }


        public IFormFile PictureUrl { get; set; }
    }
}
