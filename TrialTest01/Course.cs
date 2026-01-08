using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialTest01
{
    public delegate void myDelegate(int x, int y);
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseTitle { get; set; }
        private Dictionary<Student, double> lista = new Dictionary<Student, double>();

        public Course() { }
        public Course(int courseID, string courseTitle)
        {
            CourseID = courseID;
            CourseTitle = courseTitle;
         
        }

        public void AddStudent(Student student, double g)
        {
            var obj = lista.Keys.FirstOrDefault(x => x.StudentID == student.StudentID);
            if (obj != null)
            {
                return;
            }
            else
            {
               lista.Add(student, g);
                onNumberOfStudentChange(lista.Count - 1, lista.Count);
            }
        }
        private void onNumberOfStudentChange(int x, int y)
        {
            if(OnNumberOfStudentChange != null) OnNumberOfStudentChange(x, y);
        }
        public void RemoveStudent(int StudentID)
        {
            var obj= lista.Keys.FirstOrDefault(x => x.StudentID == StudentID);  
            if (obj != null)
            {
                lista.Remove(obj);
                onNumberOfStudentChange(lista.Count + 1, lista.Count);
            }

        }
        public override string ToString()
        {
            string s= $"Course: {CourseID} - {CourseTitle}\n";
            foreach(var item in lista.Keys)
            {
                string s1= lista[item].ToString().Replace(",","."); 
                s += $"Student: {item.StudentID} - {item.StudentName} - Grade: {s1}\n";
            }
            return s;
        }
        public event myDelegate OnNumberOfStudentChange;

    }
}
