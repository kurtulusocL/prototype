using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.EntityFramwoek;

namespace Entities.Entities
{
    public class Image : BaseEntity
    {
        public string ImageUrl { get; set; }

        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
