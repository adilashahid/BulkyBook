using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
           List<Company> objCompanies=_unitOfWork.Company.GetAll().ToList();
            return View(objCompanies);
        }
        public IActionResult Upsert(int? id)
        {
            if(id == null || id==0)
            {
                return View(new Company());
            }
            Company company=_unitOfWork.Company.Get(u=>u.Id==id);
            return View(company);
        }
        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == null)
                {
                    _unitOfWork.Company.Add(company);

                }
                else
                {
                    _unitOfWork.Company.Update(company);
                }
                _unitOfWork.Save();
                TempData["success"] = "Company Created Successfully";
                return RedirectToAction("Index");

            }
            else
            {
                return View(company);
            }
        }
            #region Api Calls

            [HttpGet]
            public IActionResult GetAll()
            {
                List<Company> companies=_unitOfWork.Company.GetAll().ToList();
                return Json(new { data = companies });

            }

            
            #endregion

        
    }
}
