using BulkyBookWebRazor_Temp.Data;
using BulkyBookWebRazor_Temp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyBookWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {


        private readonly ApplicationDbContext _context;

        public Category? Category { get; set; }
        public EditModel(ApplicationDbContext context)
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
            if (ModelState.IsValid)
            {
                _context.Categories.Update(Category);
                _context.SaveChanges();
                TempData["success"] = "Category Edit successfully";

                return RedirectToPage("Index");

            }
            return Page();
        }
    }
}
