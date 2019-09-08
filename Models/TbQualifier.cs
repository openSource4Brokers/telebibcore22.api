using System.ComponentModel.DataAnnotations;

namespace telebibcore22.api.Models
{
    public class TbQualifier
    {
        public int Id { get; set; }
        [StringLength(4)]
        public string DE { get; set; }
        [Required]
        [StringLength(10)] // Qualifier
        public string Qualifier { get; set; }
        public double? Version { get; set; }
        [StringLength(60)]
        public string Lbc1 { get; set; }
        [StringLength(60)]
        public string Lbc2 { get; set; }
        [StringLength(60)]
        public string Lbc3 { get; set; }
        [StringLength(60)]
        public string Lbc4 { get; set; }
        // public DateTime Datcre { get; set; }
        // public DateTime? Datmod { get; set; }
        // public DateTime? Datdel { get; set; }        
    }
}
