using Core.Entities.EntityFramwoek;
using Entities.Entities.User;

namespace Entities.Entities
{
    public class UnitInStock : BaseEntity
    {
        public int Quantity { get; set; }
        public string Warehouse { get; set; }

        public int? ProductId { get; set; }
        public string AppUserId { get; set; }

        public virtual Product Product { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
