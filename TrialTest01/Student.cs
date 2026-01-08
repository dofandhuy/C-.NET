using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialTest01
{
    public class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }

        public Student()
        {
            
        }
        public Student(int studentID, string studentName)
        {
            StudentID = studentID;
            StudentName = studentName;
        }
    }
}
