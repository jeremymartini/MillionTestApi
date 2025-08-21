using MongoDB.Bson;

namespace MillionTestApi.DTOS
{
    public class PropertyTraceDto : BaseDto
    {
        public string DateSale { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public ObjectId IdProperty { get; set; }
    }
}
