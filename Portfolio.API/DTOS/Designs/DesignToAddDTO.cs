namespace Portfolio.API.DTOS.Designs
{
    public class DesignToAddDTO
    {
        public string? Name { get; set; }

        public IFormFile PictureUrl { get; set; }

        public string? Link { get; set; }
        public int CategoryId { get; set; }
    }
}
