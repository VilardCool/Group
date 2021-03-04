using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplication
{
    public partial class Map
    {
        public Map()
        {
            CharacterLocations = new HashSet<CharacterLocation>();
            Sessions = new HashSet<Session>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва")]
        [StringLength(10, ErrorMessage = "Має бути не більше 10 символів")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Розмір")]
        [Range(0, 1400, ErrorMessage = "Значення має бути від 0 до 1400")]
        public int Size { get; set; }

        public virtual ICollection<CharacterLocation> CharacterLocations { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
