using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AttendanceWebApplication.Models
{
    public class AttendanceWebApplicationContext : DbContext
    {
        public AttendanceWebApplicationContext (DbContextOptions<AttendanceWebApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(new Student()
            {
                RollNumber = "A00123",
                FirstName = "hung2",
                Email = "hung@gmail.com",
                
            }, new Student()
            {
                RollNumber = "A00124",
                FirstName = "hung3",
                Email = "hung@gmail.com"
            }, new Student()
            {
                RollNumber = "A00125",
                FirstName = "hung4",
                Email = "hung@gmail.com"
            }, new Student()
            {
                RollNumber = "A00126",
                FirstName = "hung5",
                Email = "hung@gmail.com"
            }
            );

            modelBuilder.Entity<Mark>().HasData(new Mark()
            {
                Id = 1,
                SubjectMark = 9,
                SubjectName = "java",
                StudentRollNumber = "A00123"
                
            }, new Mark()
            {
                Id = 2,
                SubjectMark = 9,
                SubjectName = "Swift",
                StudentRollNumber = "A00124"

            }, new Mark()
            {
                Id = 3,
                SubjectMark = 9,
                SubjectName = "ASP.Net",
                StudentRollNumber = "A00125"

            },
            new Mark()
            {
                Id = 4,
                SubjectMark = 9,
                SubjectName = "Html/Css",
                StudentRollNumber = "A00126"

            }
            );
        }

        public DbSet<AttendanceWebApplication.Models.Student> Student { get; set; }
        public DbSet<AttendanceWebApplication.Models.Mark> Mark { get; set; }
    }
}
