using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Entities.Entities.User
{
    public class AppUserRole : IdentityRole, IEntity
    {
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public void SetActive()
        {
            IsActive = true;
        }

        public void SetCreatedDate()
        {
            CreatedDate= DateTime.Now;
        }
        public AppUserRole()
        {
            SetActive();
            SetCreatedDate();
        }
    }
}
