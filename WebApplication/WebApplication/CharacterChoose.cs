using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplication
{
    public partial class CharacterChoose
    {
        public int Id { get; set; }
        [Display(Name = "Гравець")]
        public int PlayerId { get; set; }
        [Display(Name = "Персонаж")]
        public int CharacterId { get; set; }

        [Display(Name = "Персонаж")]
        public virtual Character Character { get; set; }
        [Display(Name = "Гравець")]
        public virtual Player Player { get; set; }
    }
}
