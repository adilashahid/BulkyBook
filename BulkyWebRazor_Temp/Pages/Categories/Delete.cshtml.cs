using BulkyBookWebRazor_Temp.Data;
using BulkyBookWebRazor_Temp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyBookWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Category Category { get; set; }
        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet(int? id)
        {
            if (id != null || id == 0)
            {
                Category = _context.Categories.Find(id);

            }
        }
        public IActionResult OnPost()
        {
            Category? obj = _context.Categories.Find(Category.Id);
            if (obj == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(obj);
            _context.SaveChanges();
            TempData["success"] = "Category Delete successfully";
            return RedirectToPage("Index");

            }
          
        }
    }

