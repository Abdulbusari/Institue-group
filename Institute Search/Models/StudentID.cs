using LINQ_test.Classes;
using System.Globalization;

namespace Institute_Search.Models
{
    public class StudentID
    {
        public string range { get; set; }

        public List<Student> results;
        public string name { get; set; }
        public string nation { get; set; }

        public string grade { get; set; }
        public string course { get; set; }
        public bool intersectionMode { get; set; }


    }
}
