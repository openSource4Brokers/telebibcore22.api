using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace telebibcore22.api.Models
{
    public class TbValeur
    {
        public int Id { get; set; }
        [StringLength(4)]
        public string Code { get; set; }
        [StringLength(10)]
        public string Valeur { get; set; }
        [StringLength(20)]
        public string Lbc1 { get; set; }
        [StringLength(20)]
        public string Lbc2 { get; set; }

        [StringLength(60)]
        public string Lbl1 { get; set; }
        [StringLength(60)]
        public string Lbl2 { get; set; }
        [StringLength(60)]
        public string Lbl3 { get; set; }
        [StringLength(60)]
        public string Lbl4 { get; set; }
        // public DateTime Datcre { get; set; }
        // public DateTime? Datmod { get; set; }
        // public DateTime? Datdel { get; set; }        
    }
}
