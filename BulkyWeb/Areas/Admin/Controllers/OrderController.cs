using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model.Models;
using BulkyBook.Model.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class OrderController : Controller
	{
        [BindProperty]
        public OrderVM OrderVM {  get; set; }

        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
		{
			return View();
		}
        public IActionResult Details(int orderId)
        {
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includePropertities: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId, includePropertities: "Product")

            };
            return View(OrderVM);
        }
        [HttpPost]
        [Authorize(Roles =SD.Role_Admin+","+SD.Role_Empolyee)]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDb.Name= OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber= OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress= OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City= OrderVM.OrderHeader.City;
            orderHeaderFromDb.PostalCode= OrderVM.OrderHeader.PostalCode;
            orderHeaderFromDb.State= OrderVM.OrderHeader.State;
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier= OrderVM.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Order Updated Successfully";
            return RedirectToAction(nameof(Details),new {orderId=orderHeaderFromDb.Id});
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin+","+SD.Role_Empolyee)]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStasus(OrderVM.OrderHeader.Id,SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["success"] = "Order Updated Successfully";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });


        }
        [HttpPost]
        [ActionName("Details")]
        public IActionResult Details_PAY_NOW()
        {
            
           OrderVM.OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id, includePropertities: "ApplicationUser");
            OrderVM.OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.Id == OrderVM.OrderHeader.Id, includePropertities: "Product");
            var domin = "https://localhost:7082/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domin + $"Admin/Order/PaymentConfiramation?orderHeaderId={OrderVM.OrderHeader.Id}",
                CancelUrl = domin + "Admin/Order/Details?orderId={OrderVM.OrderHeader.Id",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };
            foreach (var item in OrderVM.OrderDetail)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }

                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePayment(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
                

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Empolyee)]
        public IActionResult ShipOrder()
        {
            var orderHeader=_unitOfWork.OrderHeader.Get(u=>u.Id==OrderVM.OrderHeader.Id);
            orderHeader.TrackingNumber= OrderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier=OrderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus=OrderVM.OrderHeader.OrderStatus;
            orderHeader.ShippingDate= OrderVM.OrderHeader.ShippingDate;
            if (orderHeader.PaymentStasus == SD.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate=DateOnly.FromDateTime(DateTime.Now.AddDays(30));
            }
            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
            TempData["success"] = "Order Updated Successfully";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });


        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Empolyee)]
        public IActionResult CancelOrder()
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
            if(orderHeader.PaymentStasus==SD.PaymentStatusApproved) {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntenId
                };
                var service = new RefundService();
                Refund refund=service.Create(options);
                _unitOfWork.OrderHeader.UpdateStasus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStasus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);


            }
            _unitOfWork.Save();
            TempData["success"] = "Order Cancelled Successfully";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });

        }
        public IActionResult PaymentConfiramation(int orderHeaderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderHeaderId);
            if (orderHeader.PaymentStasus != SD.PaymentStatusDelayedPayment)
            {
                //this is an order by customer

                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePayment(orderHeaderId, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStasus(orderHeaderId, orderHeader.OrderStatus, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
              
            }
            return View(orderHeaderId);
        }

        #region Api calls
        [HttpGet]
		public IActionResult GetAll(string status)
		{
            IEnumerable<OrderHeader> objOrderHeaders;
            if (User.IsInRole(SD.Role_Admin)|| User.IsInRole(SD.Role_Empolyee))
            {
                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includePropertities: "ApplicationUser").ToList();
            }
            else
            {
                var claimIdentity=(ClaimsIdentity)User.Identity;
                var userId=claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                objOrderHeaders= _unitOfWork.OrderHeader.GetAll(u=>u.ApplicationUserId==userId,includePropertities:"ApplicationUser");
            }
			switch (status)
			{
                case "pending":
                    objOrderHeaders = objOrderHeaders.Where(u=>u.PaymentStasus==SD.PaymentStatusDelayedPayment); 
                    break;
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }
			return Json(new { data = objOrderHeaders });
		}
		
		#endregion
	}
}
