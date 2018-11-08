using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceWebApplication.Models
{
    public class Student
    {
        [Key]
        public string RollNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public StudentStatus Status { get; set; }
        public List<Mark> Marks { get; set; }

        public Student()
        {
            this.CreateAt = DateTime.Now;
            this.UpdateAt = DateTime.Now;
            this.Status = StudentStatus.Activated;

        }
    }
    

    public enum StudentStatus
    {
        Activated = 1,
        Deactive = 0
    }
}
