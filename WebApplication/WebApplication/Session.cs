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
        public string Server { get; set; }
        [Display(Name = "Тривалість")]
        public int Duration { get; set; }
        [Display(Name = "Карта")]
        public int MapId { get; set; }
        [Display(Name = "Карта")]
        public virtual Map Map { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}
