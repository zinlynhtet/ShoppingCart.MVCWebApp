using Microsoft.AspNetCore.Mvc;
using ShoppingCart.MVCAppExamples.Models;
using System.Diagnostics;

namespace ShoppingCart.MVCAppExamples.Controllers
{
    public class HomeController : Controller
    {
        private static List<AddToCardListModel> items = new List<AddToCardListModel>();
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [ActionName("Index")]
        public IActionResult Index()
        {
            List<ItemDataModel> lst = _context.Data.ToList();
            if(lst is not null)
            {
                ItemDataResponseModel responseModel = new ItemDataResponseModel
                {
                    Data = lst
                };
                return View("Index", responseModel);
               
            }
            return NotFound("No Data Found.");
        }
        public IActionResult AddToCart()
        {
            return View(items);
        }
        [HttpPost]
        public IActionResult AddToCart(AddToCartRequestModel requestModel)
        {
           var item = items.FirstOrDefault(x => x.ItemId == requestModel.ItemId);
            if (item is null)
            {
                items ??=  new List<AddToCardListModel>();
                var itemProduct = _context.Data.FirstOrDefault(x=> x.ItemId == requestModel.ItemId);
                if (itemProduct is not null)
                {
                    items.Add(new AddToCardListModel
                    {
                        ItemId = requestModel.ItemId,
                        Name = itemProduct!.Name,
                        Quantity = 1,
                        Price = itemProduct!.Price
                    });
                }
            }
            else
            {
                item.Quantity++;
            }

            return Json(new { Count = items.Count });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(AddToCartRequestModel requestModel)
        {
            var item = items.FirstOrDefault(x => x.ItemId == requestModel.ItemId);
            if (item is null)
            goto result;

            item.Quantity--;
            if (item.Quantity == 0)
            {
                items = items.Where(x => x.ItemId != requestModel.ItemId).ToList();
            }

        result:
            return Json(new { Count = items.Count });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
