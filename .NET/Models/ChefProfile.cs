using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain
{
    public class ChefProfile
    {
        public int Id { get; set; }
        public string Bio { get; set; }
        public string AvatarThumbnailUrl { get; set; }
        public string AvatarUrl { get; set; }
        public List<LookUp> Languages { get; set; }
        public User CreatedBy { get; set; }
        public string StatusId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
