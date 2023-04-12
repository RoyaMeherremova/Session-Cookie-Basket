using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EntityFramework_Slider.Controllers
{
    public class CardController : Controller
    {
        private readonly AppDbContext _context;
        public CardController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            

            List <BasketVM> basket; 

            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]); //eyer coockide data varsa yani null deyilse coockide olan datani goturub = DeserializeObject<List<BasketVM>>.esayn edirik elmizde olan List<BasketVM>e
            }
            else
            {
                basket = new List<BasketVM>();   /*yoxdusa data teze List yaradir*/
            }

            foreach(var basketVM in basket)
            {
                Product dbProduct =_context.Products.Include(m=>m.Images).FirstOrDefault(m => m.Id == basketVM.Id);
                basketVM.Product = dbProduct;
            }
            
            return View(basket);
        }
    }
}

