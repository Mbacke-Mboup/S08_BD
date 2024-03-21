using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace S08_Labo.Models
{
    [Keyless]
    public partial class VwNbEmpl0yesParSpecialite
    {
        [StringLength(50)]
        public string Specialite { get; set; } = null!;
        [Column("NB")]
        public int? Nb { get; set; }
    }
}
