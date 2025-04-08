using System.ComponentModel.DataAnnotations;

namespace Core.Entities.EntityFramwoek
{
    public class BaseEntity : IEntity
    {
        [Key]
        public int Id { get; set; }
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
        public BaseEntity()
        {
            SetActive();
            SetCreatedDate();
        }
    }
}
