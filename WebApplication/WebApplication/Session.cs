using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplication
{
    public partial class Session
    {
        public Session()
        {
            Players = new HashSet<Player>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Сервер")]
        [StringLength(20, ErrorMessage = "Має бути не більше 20 символів")]
        public string Server { get; set; }
        [Display(Name = "Тривалість")]
        [Range(0, 600, ErrorMessage = "Значення має бути від 0 до 600")]
        public int Duration { get; set; }
        [Display(Name = "Карта")]
        public int MapId { get; set; }
        [Display(Name = "Карта")]
        public virtual Map Map { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}
