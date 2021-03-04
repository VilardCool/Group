using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

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
        [StringLength(5, ErrorMessage = "Має бути не більше 5 символів")]
        public string Model { get; set; }
        [Display(Name = "Шкода")]
        [Range(0, 450, ErrorMessage = "Значення має бути від 0 до 450")]
        public int Damage { get; set; }
        [Display(Name = "Боєзапас")]
        [Range(0, 100, ErrorMessage = "Значення має бути від 0 до 100")]
        public int Magazine { get; set; }
        [Display(Name = "Швидкострільність")]
        [Range(0, 10, ErrorMessage = "Значення має бути від 0 до 10")]
        public int RateOfFire { get; set; }

        [Display(Name = "Тип")]
        public virtual WeaponType Type { get; set; }
        public virtual ICollection<CharacterUse> CharacterUses { get; set; }
    }
}
