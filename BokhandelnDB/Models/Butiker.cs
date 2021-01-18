using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BokhandelnDB
{
    public partial class Butiker
    {
        public Butiker()
        {
            LagerSaldos = new HashSet<LagerSaldo>();
        }

        public int IdentityId { get; set; }
        public string Butiksnamn { get; set; }
        public string Address { get; set; }
        public virtual ICollection<LagerSaldo> LagerSaldos { get; set; }
    }
}
