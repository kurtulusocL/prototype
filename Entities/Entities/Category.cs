using Core.Entities.EntityFramwoek;

namespace Entities.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Subcategory> Subcategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
