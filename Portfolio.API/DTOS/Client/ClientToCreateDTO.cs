namespace Portfolio.API.DTOS.Client
{
    public class ClientToCreateDTO
    {
       
        public string Name { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public IFormFile PictureUrl { get; set; }


    }
}
