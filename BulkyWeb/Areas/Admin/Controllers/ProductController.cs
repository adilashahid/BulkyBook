using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model.Models;
using BulkyBook.Model.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includePropertities:"Category").ToList();
            
            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
        
            
            //ViewBag.CategoryList = CategoryList;
           
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u =>
                new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

                Product = new Product()
            };
            if(id==null || id==null)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM obj,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwRootPath = _webHostEnvironment.WebRootPath;
                if (file!=null)

                    {

                     string fileNmae=Guid.NewGuid().ToString()+ Path.GetExtension(file.FileName);
                     string productPath = Path.Combine(wwRootPath, @"Images\Product");
                    if(!String.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        //delete old image
                        var oldImgPath=Path.Combine(wwRootPath,obj.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);  
                        }
                              }
                    using (var filestream=new FileStream(Path.Combine(productPath,fileNmae),FileMode.Create))
                       {
                        file.CopyTo(filestream);
                         }
                  obj.Product.ImageUrl = @"\Images\Product\" + fileNmae;
            }
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                  
                }
                else
                { 
                    _unitOfWork.Product.Update(obj.Product);
                  
                }

                _unitOfWork.Save();
                TempData["success"] = "Category Created successfully";
                return RedirectToAction("Index");

            }
            else
            {

                obj.CategoryList = _unitOfWork.Category.GetAll().Select(u =>
                new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(obj);


            }
          
           

        }
        
        
        #region Api calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includePropertities: "Category").ToList();

            return Json(new {data= objProductList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted=_unitOfWork.Product.Get(u=>u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "error while deleting" });
            }
            var oldImgPath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImgPath))
            {
                System.IO.File.Delete(oldImgPath);
            }
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfully" });
        }
        public IActionResult Delete()
        {
            var companyToBeDeleted= _unitOfWork.Company.Get(u=>u.Id==0);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message="Error while Deleting" });
            }
            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Succesfully" });
        }

        #endregion
    }

}
