using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BokhandelnDB
{
    public partial class Böcker
    {
        public Böcker()
        {
            LagerSaldos = new HashSet<LagerSaldo>();
        }


        public string Isbn13 { get; set; } 
        public string Titel { get; set; }
        public string Språk { get; set; }
        public int? Pris { get; set; }
        public string Utgivningsdatum { get; set; }
        public int? FörfattareId { get; set; }
        public string ViktGram { get; set; }
        public int? Sidor { get; set; }

        public virtual Författare Författare { get; set; }
        public virtual ICollection<LagerSaldo> LagerSaldos { get; set; }
        public object This { get {return this; } }
    }
}
