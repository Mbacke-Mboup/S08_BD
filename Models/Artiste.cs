using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace S08_Labo.Models
{
    [Table("Artiste", Schema = "Employes")]
    public partial class Artiste
    {
        [Key]
        [Column("ArtisteID")]
        public int ArtisteId { get; set; }
        [StringLength(50)]
        public string Specialite { get; set; } = null!;
        [Column("EmployeID")]
        public int EmployeId { get; set; }

        [ForeignKey("EmployeId")]
        [InverseProperty("Artistes")]
        public virtual Employe Employe { get; set; } = null!;
    }
}
