using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class RoleAssignVM
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool HasAssign { get; set; }
    }
}
