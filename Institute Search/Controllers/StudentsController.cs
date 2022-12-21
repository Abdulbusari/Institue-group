using Microsoft.AspNetCore.Mvc;
using LINQ_test.Classes;

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
    }
}
