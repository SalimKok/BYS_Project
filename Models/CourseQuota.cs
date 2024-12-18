using Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class CourseQuota
    {
        [Key]
        public int CourseId { get; set; }
        public int Quota { get; set; }
        public int RemainingQuota { get; set; }


        public virtual Course Course { get; set; }
    }
}
