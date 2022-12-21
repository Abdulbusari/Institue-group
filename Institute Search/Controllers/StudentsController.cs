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
            return View();
        }

        [HttpPost]
        public IActionResult Search(/* you can change the parameters */ StudentSearchForm query)
        {
            // var result = Repo.......
            return View(/* result */);
        }
    }
}
