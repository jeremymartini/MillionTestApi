using MongoDB.Bson;

namespace MillionTestApi.DTOS
{
    public class OwnerDto : BaseDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Photo { get; set; } = null!;
        public string Birthday { get; set; } = null!;
    }
}
