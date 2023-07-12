using FranchiseMenu.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FranchiseMenu.ENTITY.Concrete
{
    public class SecurityHistory : IEntity
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string TokenString { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool Status { get; set; }
    }
}
