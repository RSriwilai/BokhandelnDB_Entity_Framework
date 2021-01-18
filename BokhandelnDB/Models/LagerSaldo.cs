using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BokhandelnDB
{
    public partial class LagerSaldo
    {

        public int ButikId { get; set; }
        public string Isbn { get; set; } 
        public int? Antal { get; set; }
        public virtual Butiker Butik { get; set; }
        public virtual Böcker IsbnNavigation { get; set; }


    }
}
