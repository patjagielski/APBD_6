using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tutorial_3._1.Models;

namespace Tutorial_3._1.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {

        [HttpGet]
        public IActionResult CreateStudent()
        {
            
            var students = new List<Student>();
            using (var sqlConnection = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s19696;Integrated Security=True"))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    string query = "select s.FirstName, s.LastName, s.IndexNumber, s.Birthdate, st.Name as Studies, e.Semester" +
                                " from Student s join Enrollment e " +
                                " on e.IdEnrollment = s.IdEnrollment join Studies st" +
                                " on st.IdStudy = e.IdStudy;";
                    command.CommandText = (query);
                    sqlConnection.Open();
                    var response = command.ExecuteReader();
                    while (response.Read())
                    {
                        var st = new Student();
                        st.FirstName = response["FirstName"].ToString();
                        st.LastName = response["LastName"].ToString();
                        st.Studies = response["Studies"].ToString();
                        st.IndexNumber = response["IndexNumber"].ToString();
                        st.BirthDate = DateTime.Parse(response["Birthdate"].ToString());
                        st.Semester = int.Parse(response["Semester"].ToString());

                        students.Add(st);
                    }
                }
                return Ok(students);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            string studies = "";
            using (var client = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s19696;Integrated Security=True"))
            {
                using (var con = new SqlCommand("StudentsForStudies", client) {CommandType = System.Data.CommandType.StoredProcedure})
                {
                    con.Parameters.Add(new SqlParameter("@Studies", studies));
                    con.Parameters.Add(new SqlParameter("@Studies", studies));
                    client.Open();
                    con.Connection = client;
                    con.CommandText = "select a.IndexNumber, a.FirstName, a.LastName,b.semester,c.Name" +
                                " from Student a join Enrollment b" +
                                " on a.IdEnrollment = b.IdEnrollment join Studies c" +
                                " on c.idStudy = b.IdStudy where a.IndexNumber = @id;";
                    con.Parameters.AddWithValue("id", id);
                    
                    var reader = con.ExecuteReader();
                    while (reader.Read())
                    {
                        studies = $"First Name:{reader["FirstName"]} \n" +
                            $"Last Name: {reader["LastName"]} \n" +
                            $"Index Number: {reader["IndexNumber"]} \n" +
                            $"Semester:{reader["Semester"]} \n" +
                            $"Studies:{reader["Name"]} \n";
                    }

                }

            }
            return Ok(studies);

        }

        [HttpDelete]
        public IActionResult DeleteStudent()
        {
            string temp = "";
            using (var client = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s19696;Integrated Security=True"))
            {
                
                using(var command = new SqlCommand())
                {
                    command.Connection = client;
                    string query = "Delete Student;";
                    command.CommandText = (query);
                    client.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        temp = $"{ reader["Student"]}";
                    }
                }

            }
            return Ok("Table Student Deleted");

        }
      
       


        /*  [HttpPost]
          public IActionResult CreateStudent(Models.Student student)
          {
              student.IndexNumber = $"s{new Random().Next(1, 20000)}";

              return Ok(student);

          }
          [HttpPut]
          public IActionResult PutStudent([FromBody] Models.Student student)
          {
              student.FirstName = "Mark";

              return Ok("Update complete");

          }
          [HttpDelete]
          public IActionResult DeleteStudent([FromBody] Models.Student student)
          {
              if(student.IdStudent == 1)
              {
                  return Ok("Delete complete");
              }
              else
              {
                  return NotFound("That student id does not exist");
              }
          }
      }*/

    }
}
