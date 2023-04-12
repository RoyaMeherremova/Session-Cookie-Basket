using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace EntityFramework_Slider.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet] 
        public async Task<IActionResult> Index()
        {

            //HttpContext.Session.SetString("name", "Pervin");   //Seyfe acilan kimi sessiona sesiona data qoyuram


            //Response.Cookies.Append("surname", "Rehimli", new CookieOptions { MaxAge = TimeSpan.FromMinutes(30) }); //Cocikye data yerlesdirmey key adi ve valusu.TimeSpan-geyd edirik nece degeden sonra silinsin ordan bu cockie.TimeSpan-vaxt ferqi tapmaq ucun istifade olunur  

            //Book book = new Book
            //{
            //    Id = 1,
            //    Name = "Xosrov ve Shirin"
            //};

            //Response.Cookies.Append("book",JsonConvert.SerializeObject(book));    /*JsonConvert.SerializeObject=Objecti Jsona ceviir.Json bize string verir deye Jsona cevirib Coockiye yerlesdiririk.Coockie bizden Int ve string gebul edir*/

            //Linkque querileri =Blogs.Where(m => !m.SoftDelete)-Silinimeyen bloglari ver

            //List miras alir IIEnumerabldan.List-in methodlari var IEnumerable-un yoxdu.Datani viewa IEnumerable kimi gondermek yaxsdidir cunku- 
            //elave methodlar yoxdu daha suretli isleyir

            //IQueryable-query yaradiriq RAMda saxlayiriq hele data getirmirik
            //IQueryable<Slider> slide = _context.Sliders.AsQueryable();
            //sonra serte uyqun datani getiririk.ToList()-yazdiqda request gedir DataBazaya
            //List<Slider> query = slide.Where(m => m.Id >5).ToList();

            //List<int> nums = new List<int>() { 1, 2, 4, 5, 6 };
            //FirstOrDefault()--sertde gondermek olur ,serte uyqun datadan bir necedenedise 1-cini gorsedir,yoxdusa default deyerini gorsedir
            //var res = nums.FirstOrDefault(m => m ==3);

            //First()--varsa data serte uyqunun verir,yoxdusa exception cixarir
            //var res2 = nums.First(m => m == 3);

            //SingleOrDefault()--serte uyqun data  birdene varsa verir,bir necedenedise exception verir,yoxdusa data default deyerin verir
            //Single()--serte uyqun data  birdene varsa verir,bir necedenedise exception verir,yoxdusa exception verir
            //var res = nums.SingleOrDefault(m => m == 3);          
            //ViewBag.num = res;



            List<Slider> sliders = await _context.Sliders.ToListAsync();

            SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync();

            IEnumerable<Blog> blogs = await _context.Blogs.Where(m => !m.SoftDelete).ToListAsync();

            IEnumerable<Category> categories = await _context.Categories.Where(m => !m.SoftDelete).ToListAsync();


            //Inlude()--Relation  qurduqumuz tablerda istifade edirik.-Tablin icindeki basqa table catmaq istirsense.Many olan terefe.

            IEnumerable<Product> products = await _context.Products.Include(m => m.Images).Where(m => !m.SoftDelete).ToListAsync();

            About abouts = await _context.Abouts.Include(m => m.Adventages).FirstOrDefaultAsync();

            IEnumerable<Experts> experts = await _context.Experts.Where(m => !m.SoftDelete).ToListAsync();

            ExpertsHeader expertsheaders = await _context.ExpertsHeaders.FirstOrDefaultAsync();

            Subscribe subscribs = await _context.Subscribs.FirstOrDefaultAsync();

            BlogHeader blogheaders = await _context.BlogHeaders.FirstOrDefaultAsync();
            IEnumerable<Say> says = await _context.Says.Where(m => !m.SoftDelete).ToListAsync();

            IEnumerable<Instagram> instagrams = await _context.Instagrams.Where(m => !m.SoftDelete).ToListAsync();

            HomeVM model = new()
            {
                Sliders = sliders,
                SliderInfo = sliderInfo,
                Blogs = blogs,
                Categories = categories,
                Products = products,
                Abouts = abouts,
                Experts = experts,
                ExpertsHeaders = expertsheaders,
                Subscribs = subscribs,
                BlogHeaders = blogheaders,
                Says = says,
                Instagrams = instagrams
            };

            return View(model);
        }



        //public IActionResult Test()  /* stroragden datani goturub UI-a gorseden method*/
        //{

        //    var sessionData = HttpContext.Session.GetString("name");  //Sessionda olan datani gotururuk keyine gore
        //    var cokieData = Request.Cookies["surname"];  //cockiyede olan datani gotururuk keyine gore
        //    var objectData = JsonConvert.DeserializeObject<Book>(Request.Cookies["book"]);       /*JsonConvert.DeserializeObject < Book >. DeserializeObject-Json -DuplicateWaitObjectException cevirmek -Version tipini geyd edirik Book olsun*/


        //    return Json(objectData);
        //}


        [HttpPost] /*Post yani data daxil edirik*/ 
        [ValidateAntiForgeryToken]  //yoxlayir bize gelen Token bu methoda gire biler ya yox,yani bu saytdan giribse user methodu islet.
        public async Task<IActionResult> AddBasket(int? id)
        {

            //eyer id silinirse URL-den
            if (id == null) return BadRequest();

             Product dbProduct = await _context.Products.FindAsync(id);

            /*eyer URL-de databazada olmayan Id yazilarsa*/
            if (dbProduct == null) return NotFound();

            //BasketVM-yaradiriq cunku bize hemin Listden yaliz bir nece data lazimdir
            //Ve Coockiye List<BasketVM> gonderik cunku bir nece data gelecek Coockiye
            
            List<BasketVM> basket; /*Bos List teyin edirik*/

            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]); //eyer coockide data varsa yani null deyilse coockide olan datani goturub = DeserializeObject<List<BasketVM>>.esayn edirik elmizde olan List<BasketVM>e
            }
            else
            {
                basket = new List<BasketVM>();   /*yoxdusa data teze List yaradir*/
            }

            BasketVM? existProduct = basket.FirstOrDefault(m=>m.Id == dbProduct.Id);  //existProduct=Finf(elmizdeki List<basketVM> Id-si = databazda olan data Id)

            if(existProduct == null)
            {
                basket?.Add(new BasketVM
                {    //Liste Add edirik yeni BasketVM yani yeni data
                    Id = dbProduct.Id,
                    Count = 1
                });
            }
            else
            {
                existProduct.Count++;
            }

            



            Response.Cookies.Append("basket",JsonConvert.SerializeObject(basket));  //ve append edirik Coockiye
            return RedirectToAction(nameof(Index));  /*basqa actiona yonlendiririk*/
        }


    }

    //class Book
    //{
    //    public int Id { get; set;}

    //    public string Name { get; set; }
    //}
}