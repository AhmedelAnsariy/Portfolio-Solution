using Portfolio.API.DTOS.Designs;
using Portfolio.Core.Models;

namespace Portfolio.API.DTOS.Category
{
    public class CategoryToReturnDto
    {

        public int Id { get; set; }
        public string Name { get; set; }

       


        public ICollection<DesignToReturnDto> Designs { get; set; }

    }
}
