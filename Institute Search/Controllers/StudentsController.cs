using Microsoft.AspNetCore.Mvc;
using LINQ_test.Classes;
using Institute_Search.Models;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;

namespace Institute_Search.Controllers
{
    public class StudentsController : Controller
    {
        Institute Repo = Institute.GetInstance();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(Repo.Students);
        }


        public IActionResult Search()
        {
            return View(new StudentID());
        }


        [HttpPost]
        public IActionResult Search(StudentID sd)
        {
            sd.results = Repo.Students.ToList();
            if (!string.IsNullOrWhiteSpace(sd.range))
            {
                if (sd.range.Contains("<"))
                {
                    sd.range = sd.range.Replace('<', ' ');
                    sd.results = sd.results.Where(s => s.Id <= int.Parse(sd.range)).ToList();
                }
                else if (sd.range.Contains(">"))
                {
                    sd.range = sd.range.Replace('>', ' ');
                    sd.results = sd.results.Where(s => s.Id >= int.Parse(sd.range)).ToList();
                }
                else if (sd.range.Contains("-"))
                {
                    sd.range = sd.range.Replace('-', ' ');
                    List<string> ranges = sd.range.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                    sd.results = sd.results.Where(s => s.Id >= int.Parse(ranges[0]) && s.Id <= int.Parse(ranges[1])).ToList();
                }
            }

            if (!string.IsNullOrWhiteSpace(sd.name))
            {
                 string[] qs = sd.name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                 IEnumerable<Student> sts = sd.results.Where(s => qs.All(t => $"{s.FirstName} {s.LastName}".ToLower().Contains(t)));
                sd.results = sts.ToList();
            }


            if (!string.IsNullOrWhiteSpace(sd.nation))
            {
    //            for(var student in sd.results)
                for(int i = 0; i < sd.results.Count(); i++)
                {
                    //if (!sd.results[i].Nationality.ToLower().Contains(sd.nation.ToLower()))
                    if (!sd.nation.ToLower().Contains(sd.results[i].Nationality.ToLower()))
                    {
                        sd.results.Remove(sd.results[i]);
                        i--;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(sd.grade))
            {
                //            for(var student in sd.results)
                for (int i = 0; i < sd.results.Count(); i++)
                {
                    //if (!sd.results[i].Nationality.ToLower().Contains(sd.nation.ToLower()))
                    if (!sd.grade.Contains(sd.results[i].GradeYear.ToString()))
                    {
                        sd.results.Remove(sd.results[i]);
                        i--;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(sd.course))
            {
               if(sd.intersectionMode == false)
                {
                    /* Pseudo Code
                     * split courses by " "
                     * Loop through students
                     * bool false
                         * loop through course of students
                             * loop through split courses
                                * if(course.name.contains(split name)) 
                                    * bool = true
                                    * break
                                    * 
                                *if (bool)
                                * break
                     * if(!bool) 
                     * remove the student
                     * i --;
                     */
                    List<string> input = sd.course.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                    for (int i = 0; i < sd.results.Count(); i++)
                    {
                        bool isValid = false;
                        foreach(var cs in sd.results[i].ClassroomStudents)
                        {
                            foreach(var course in input)
                            {
                                if (cs.Classroom.Course.Name.ToLower().Contains(course.ToLower()))
                                {
                                    isValid = true;
                                    break;
                                }
                            }
                            if(isValid)
                            {
                                break;
                            }
                        }
                        if(!isValid)
                        {
                            sd.results.RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                { 
                    List<string> filters = sd.course.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<Student> result = new List<Student>();

                    foreach(var student in sd.results)
                    {
                        int count = 0;
                        foreach(var filter in filters)
                        {
                            IEnumerable<string> courseNames = student.ClassroomStudents.Select(cs => cs.Classroom.Course.Name.ToLower());
                            foreach(var course in courseNames)
                            {
                                if(course.Contains(filter.ToLower()))
                                {
                                    count++;
                                    break;
                                }
                            }
                        }
                        if(count == filters.Count())
                        {
                            result.Add(student);
                        }
                    }

                    /*     for (int i = 0; i < sd.results.Count(); i++)
                         {
                             foreach(var student in result)
                             {
                                 if (sd.results[i] != student)
                                 {
                                     sd.results.RemoveAt(i);
                                     i--;
                                     break;
                                 }
                             }
                         }*/
                    sd.results = result;
                }
            }
            return View(sd);
        }
    }
}



/*
     for(int i = 0; i < sd.results.Count(); i++)
                    {
                        List<string> courseCheckList = sd.course.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                        bool courseCheck = false;
                        foreach (var hashsetCS in sd.results[i].ClassroomStudents )
                        {
                            if (courseCheckList[0].Contains(sd.results[i].ClassroomStudents.ToList()[j].Classroom.Course.Name))
                            {

                                courseCheckList.RemoveAt(0);
                                
                            }
                            /*       if(hashsetCS.Classroom.Course.Name.ToLower().Contains())
                                   //if (sd.course.ToLower().Contains(hashsetCS.Classroom.Course.Name.ToLower()))
                                   {
                                       courseCheck = true;
                                       break;
                                   }*/


/*
                     for (int i = 0; i < sd.results.Count() - 1 ; i++)
                    {
                        List<string> courseCheckList = sd.course.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                        for(int j = 0; j < sd.results[i].ClassroomStudents.Count(); j++)
                        {
                            if (courseCheckList.Count() == 0)
                            {
                                break;
                            }
                            
                            if (sd.results[i].ClassroomStudents.ToList()[j].Classroom.Course.Name.ToLower()
                                .Contains(courseCheckList[0].ToLower()))
                           //if (courseCheckList[0].ToLower().Contains(sd.results[i].ClassroomStudents.ToList()[j].Classroom.Course.Name.ToLower()))
                            {
                  
                                courseCheckList.RemoveAt(0);
                                j = 0;
                            }
                        }
                        if (courseCheckList.Count() == 0)
                        {
                            sd.results.RemoveAt(i);
                        }

                    }
 */