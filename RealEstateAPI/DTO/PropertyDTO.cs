namespace RealEstateAPI.DTO
{
    public class PropertyDTO
    {

        public string Name { get; set; }

        public string Details { get; set; }

        public string Address { get; set; }

        public string ImageUrel { get; set; }

        public long price { get; set; }

        public bool IsTrending { get; set; }

        public int CategoryId { get; set; }

        public int UserId { get; set; }
    }
}
