
using System.ComponentModel.DataAnnotations;

namespace Core.Email
{
    public class ConfirmEmailDto
    {
        [Required, DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        public int ConfirmCode { get; set; }
    }
}
