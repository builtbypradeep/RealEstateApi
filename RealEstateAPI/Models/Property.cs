namespace RealEstateAPI.Models
{
    public class Property
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }

        public string Address { get; set; }

        public string ImageUrel {  get; set; }  

        public long price { get; set; }

        public bool IsTrending { get; set; }

        public int CategoryId  { get; set; }

        public Category category { get; set; }

        public int UserId { get; set; }

        public User user { get; set; }  
    }
}
