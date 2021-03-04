using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplication
{
    public partial class Player
    {
        public Player()
        {
            CharacterChooses = new HashSet<CharacterChoose>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Логін")]
        [StringLength(10, ErrorMessage = "Має бути не більше 10 символів")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Пароль")]
        [StringLength(10, ErrorMessage = "Має бути не більше 10 символів")]
        public string Password { get; set; }
        [Display(Name = "Нікнейм")]
        [StringLength(10, ErrorMessage = "Має бути не більше 10 символів")]
        public string Nickname { get; set; }
        [Display(Name = "Сесія")]
        public int SessionId { get; set; }
        [Display(Name = "Сесія")]
        public virtual Session Session { get; set; }
        public virtual ICollection<CharacterChoose> CharacterChooses { get; set; }
    }
}
