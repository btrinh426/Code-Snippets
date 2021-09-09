using Sabio.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests
{
    public class ChefProfileAddRequest
    {
        [Required]
        [StringLength (4000, MinimumLength = 2)]
        public string Bio { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 2), Url]
        public string AvatarThumbnailUrl { get; set; }

        [Required]
        [StringLength (255, MinimumLength = 2), Url]
        public string AvatarUrl { get; set; }


    }
}
