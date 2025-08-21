using MongoDB.Bson;

namespace MillionTestApi.DTOS
{
    public class PropertyDto : BaseDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Price { get; set; }
        public int CodeInternal { get; set; }
        public int Year { get; set; }
        public ObjectId IdOwner { get; set; }
    }
}
