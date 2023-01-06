using Microsoft.AspNetCore.Mvc;
using LINQ_test.Classes;
using Institute_Search.Models;

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
            return View(sd);
        }
    }
}
