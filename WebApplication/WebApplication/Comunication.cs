using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplication
{
    public partial class Comunication
    {
        public int Id { get; set; }
        [Display(Name = "Персонаж 1")]
        public int Character1Id { get; set; }
        [Display(Name = "Персонаж 2")]
        public int Character2Id { get; set; }

        [Display(Name = "Персонаж 1")]
        public virtual Character Character1 { get; set; }
        [Display(Name = "Персонаж 2")]
        public virtual Character Character2 { get; set; }
    }
}
