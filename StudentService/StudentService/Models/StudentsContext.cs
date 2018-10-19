using System.Data.Entity;

namespace StudentService.Models
{
    public class StudentsContext : DbContext
    {
        public StudentsContext()
            : base("name=StudentsContext")
        {

        }
        public DbSet<Student> Students { get; set; }
    }
}