using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using StudentService.Models;

namespace StudentService.Controllers
{
    public class StudentsController : ODataController
    {
        StudentsContext db = new StudentsContext();
        private bool StudentExists(int key)
        {
            return db.Students.Any(p => p.Id == key);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //returns all students
        [EnableQuery]
        public IQueryable<Student> Get()
        {
            return db.Students;
        }

        //query the entity set
        [EnableQuery]
        public SingleResult<Student> Get([FromODataUri] int key)
        {
            IQueryable<Student> result = db.Students.Where(s => s.Id == key);
            return SingleResult.Create(result);
        }

        //add a student to the set 
        public async Task<IHttpActionResult> Post(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Students.Add(student);
            await db.SaveChangesAsync();
            return Created(student);
        }

        //update an entity - PUT or PATCH - patch preferred 
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Student> student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.Students.FindAsync(key);

            if (entity == null)
            {
                return NotFound();
            }

            student.Patch(entity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(entity);
        }

        // delete student 
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var student = await db.Students.FindAsync(key);
            if (student == null)
            {
                return NotFound();
            }

            db.Students.Remove(student);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

    }
}