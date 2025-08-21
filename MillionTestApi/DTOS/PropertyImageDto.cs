using MongoDB.Bson;

namespace MillionTestApi.DTOS
{
    public class PropertyImageDto : BaseDto
    {
        public ObjectId IdProperty { get; set; }
        public string File { get; set; } = null!;
        public bool Enabled { get; set; }
    }
}
