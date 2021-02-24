using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplication
{
    public partial class WeaponType
    {
        public WeaponType()
        {
            Weapons = new HashSet<Weapon>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage ="Поле не повинно бути порожнім")]
        [Display(Name = "Тип")]
        public string Name { get; set; }

        public virtual ICollection<Weapon> Weapons { get; set; }
    }
}
