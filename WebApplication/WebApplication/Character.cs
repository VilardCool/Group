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
        public int Playable { get { return play; } set { if (value != 0) play = 1; else play = 0; } }
        private int play;
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "Здоров'я")]
        public int Health { get; set; }
        [Display(Name = "Витривалість")]
        public int? Stamina { get; set; }
        [Display(Name = "Кількість ліків")]
        public int? Backpack { get; set; }

        public virtual ICollection<CharacterChoose> CharacterChooses { get; set; }
        public virtual ICollection<CharacterLocation> CharacterLocations { get; set; }
        public virtual ICollection<CharacterUse> CharacterUses { get; set; }
        public virtual ICollection<Comunication> ComunicationCharacter1s { get; set; }
        public virtual ICollection<Comunication> ComunicationCharacter2s { get; set; }
    }
}
