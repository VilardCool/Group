using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplication
{
    public partial class CharacterUse
    {
        public int Id { get; set; }
        [Display(Name = "Персонаж")]
        public int CharacterId { get; set; }
        [Display(Name = "Зброя")]
        public int WeaponId { get; set; }

        [Display(Name = "Персонаж")]
        public virtual Character Character { get; set; }
        [Display(Name = "Зброя")]
        public virtual Weapon Weapon { get; set; }
    }
}
