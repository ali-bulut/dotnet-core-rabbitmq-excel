using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQExcelApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RabbitMQExcelApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateProductExcel()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var filename = $"product-excel-{Guid.NewGuid().ToString().Substring(1, 10)}";

            UserFile userFile = new UserFile()
            {
                UserId = user.Id,
                FileName = filename,
                FileStatus = FileStatus.Creating
            };

            await _context.UserFiles.AddAsync(userFile);
            await _context.SaveChangesAsync();

            // TODO: send message to queue
            TempData["StartCreatingExcel"] = true;

            return RedirectToAction(nameof(Files));
        }

        public async Task<IActionResult> Files()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var data = await _context.UserFiles.Where(x => x.UserId == user.Id).ToListAsync();

            return View(data);
        }
    }
}
