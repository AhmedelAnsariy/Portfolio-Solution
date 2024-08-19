using Portfolio.Core.Models;

namespace Portfolio.API.DTOS.Designs
{
    public class DesignToReturnDto
    {

        public int Id { get; set; }
        public string? Name { get; set; }

        public string PictureUrl { get; set; }

        //public string? Link { get; set; }

        public string Category { get; set; }
        //public int CategoryId { get; set; }
    }
}
