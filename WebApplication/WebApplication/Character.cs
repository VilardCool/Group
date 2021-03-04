using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplication
{
    public partial class Character
    {
        public Character()
        {
            CharacterChooses = new HashSet<CharacterChoose>();
            CharacterLocations = new HashSet<CharacterLocation>();
            CharacterUses = new HashSet<CharacterUse>();
            ComunicationCharacter1s = new HashSet<Comunication>();
            ComunicationCharacter2s = new HashSet<Comunication>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Ігровий")]
        [Range(0,1, ErrorMessage = "Значення має бути 0 або 1")]
        public int Playable { get { return play; } set { if (value != 0) play = 1; else play = 0; } }
        private int play;
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Ім'я")]
        [StringLength(10, ErrorMessage = "Має бути не більше 10 символів")]
        public string Name { get; set; }
        [Display(Name = "Здоров'я")]
        [Range(0, 100, ErrorMessage = "Значення має бути від 0 до 100")]
        public int Health { get; set; }
        [Display(Name = "Витривалість")]
        [Range(0, 100, ErrorMessage = "Значення має бути від 0 до 100")]
        public int? Stamina { get; set; }
        [Display(Name = "Кількість ліків")]
        [Range(0, 3, ErrorMessage = "Значення має бути від 0 до 3")]
        public int? Backpack { get; set; }

        public virtual ICollection<CharacterChoose> CharacterChooses { get; set; }
        public virtual ICollection<CharacterLocation> CharacterLocations { get; set; }
        public virtual ICollection<CharacterUse> CharacterUses { get; set; }
        public virtual ICollection<Comunication> ComunicationCharacter1s { get; set; }
        public virtual ICollection<Comunication> ComunicationCharacter2s { get; set; }
    }
}
