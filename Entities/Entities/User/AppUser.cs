using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Entities.Entities.User
{
    public class AppUser : IdentityUser, IEntity
    {
        public string NameSurname { get; set; }
        public int? ConfirmCode { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<UnitInStock> UnitInStocks { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public void SetActive()
        {
            IsActive = true;
        }

        public void SetCreatedDate()
        {
            CreatedDate = DateTime.Now;
        }
        public AppUser()
        {
            SetActive();
            SetCreatedDate();
            EmailConfirmed = true;
        }
    }
}
