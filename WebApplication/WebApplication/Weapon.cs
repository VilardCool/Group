using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplication
{
    public partial class Weapon
    {
        public Weapon()
        {
            CharacterUses = new HashSet<CharacterUse>();
        }

        public int Id { get; set; }
        public int TypeId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Модель")]
        public string Model { get; set; }
        [Display(Name = "Шкода")]
        public int Damage { get; set; }
        [Display(Name = "Боєзапас")]
        public int Magazine { get; set; }
        [Display(Name = "Швидкострільність")]
        public int RateOfFire { get; set; }

        [Display(Name = "Тип")]
        public virtual WeaponType Type { get; set; }
        public virtual ICollection<CharacterUse> CharacterUses { get; set; }
    }
}
