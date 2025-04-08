using Core.Entities.EntityFramwoek;

namespace Entities.Entities
{
    public class Subcategory : BaseEntity
    {
        public string Name { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
