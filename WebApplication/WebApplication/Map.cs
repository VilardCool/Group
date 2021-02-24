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
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Розмір")]
        public int Size { get; set; }

        public virtual ICollection<CharacterLocation> CharacterLocations { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
