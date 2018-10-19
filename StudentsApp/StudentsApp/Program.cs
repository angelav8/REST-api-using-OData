using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp
{
    class Program
    {

        // Get an entire entity set.
        static void ListAllStudents(Default.Container container)
        {
            foreach (var s in container.Students)
            {
                Console.WriteLine("{0} {1} {2}", s.FirstName, s.LastName, s.Subject);
            }
        }

        static void AddStudent(Default.Container container, StudentService.Models.Student student)
        {
            container.AddToStudents(student);
            var serviceResponse = container.SaveChanges();
            foreach (var operationResponse in serviceResponse)
            {
                Console.WriteLine("Response: {0}", operationResponse.StatusCode);
            }
        }

        static void Main(string[] args)
        {
            // TODO: Replace with your local URI.
            string serviceUri = "http://localhost:62338/";
            var container = new Default.Container(new Uri(serviceUri));

            var student = new StudentService.Models.Student()
            {
                FirstName = "Angela",
                LastName = "Lavery",
                Subject = "Software Engineering"
            };

            AddStudent(container, student);
            ListAllStudents(container);
        }
    }
}
