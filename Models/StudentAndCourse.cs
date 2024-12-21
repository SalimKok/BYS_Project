using Project.Models;

namespace Project.Models
{
    public class StudentAndCourse
    {
        public Student Student { get; set; }

        public List<Course> Course { get; set; }
    }
}