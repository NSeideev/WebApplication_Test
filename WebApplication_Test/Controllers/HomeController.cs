using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using WebApplication_Test.Data;
using WebApplication_Test.Models;

namespace WebApplication_Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _context;

        public HomeController(ILogger<HomeController> logger,AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] Models.Task formData)
        {
            try
            {
                if (_context.tasks.FirstOrDefault(t => t.Name == formData.Name) == null)
                {
                    // ������� ����� ������
                    var task = new Models.Task
                    {
                        Name = formData.Name,
                        Description = formData.Description,
                        CreatedDate = formData.CreatedDate,
                        Priority = formData.Priority,
                    };

                    _context.tasks.Add(task);
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                else return StatusCode(500, "����� ������ ��� ����������");
            }
            catch (Exception ex)
            {
                // ���� ��������� ������, ������ ������ �������
                return StatusCode(500, "������ ��� �������� ������: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _context.tasks.Where(t => t.isDone == false).ToListAsync();
            return Ok(tasks); // ���������� ������ ����� � ������� JSON
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsDone(string taskname)
        {

            // ������� ������ � ���� ������ �� �����
            var task = _context.tasks.Where(t => t.Name == taskname).First();

            task.isDone = true;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
