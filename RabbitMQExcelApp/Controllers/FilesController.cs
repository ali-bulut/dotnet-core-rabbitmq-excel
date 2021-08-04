using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQExcelApp.Models;

namespace RabbitMQExcelApp.Controllers
{
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly AppDbContext _context;

        public FilesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int fileId)
        {
            if (file.Length <= 0) return BadRequest();

            var userFile = await _context.UserFiles.FirstAsync(x => x.Id == fileId);
            var filePath = userFile.FileName + Path.GetExtension(file.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", filePath);

            using FileStream stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            userFile.CreatedDate = DateTime.Now;
            userFile.FilePath = filePath;
            userFile.FileStatus = FileStatus.Completed;

            await _context.SaveChangesAsync();

            // TODO: SignalR Implementation

            return Ok();
        }
    }
}
