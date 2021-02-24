using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplication
{
    public partial class CharacterLocation
    {
        public int Id { get; set; }
        [Display(Name = "Персонаж")]
        public int CharacterId { get; set; }
        [Display(Name = "Карта")]
        public int MapId { get; set; }
        [Display(Name = "Позиція по X")]
        public int? PositionX { get; set; }
        [Display(Name = "Позиція по Y")]
        public int? PositionY { get; set; }

        [Display(Name = "Персонаж")]
        public virtual Character Character { get; set; }
        [Display(Name = "Карта")]
        public virtual Map Map { get; set; }
    }
}
