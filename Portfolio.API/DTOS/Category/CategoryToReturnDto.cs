using Portfolio.API.DTOS.Designs;
using Portfolio.Core.Models;

namespace Portfolio.API.DTOS.Category
{
    public class CategoryToReturnDto
    {
        public string Name { get; set; }

        public string? Code { get; set; }


        public ICollection<DesignToReturnDto> Designs { get; set; }

    }
}
