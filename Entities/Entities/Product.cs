using Core.Entities.EntityFramwoek;
using Entities.Entities.User;

namespace Entities.Entities
{
    public class Product : BaseEntity
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public string AppUserId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Subcategory Subcategory { get; set; }
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<UnitInStock> UnitInStocks { get; set; }
    }
}
