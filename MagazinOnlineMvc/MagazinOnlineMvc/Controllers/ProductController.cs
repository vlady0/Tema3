using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagazinOnlineMvc.Models;
using System.Text;
using MagazinOnlineMvc.Exporter;


namespace MagazinOnlineMvc.Controllers
{
    public class ProductController : Controller
    {
        private ProductDBContext db = new ProductDBContext();
        private Cart c;


        #region CRUD
        
        //
        // GET: /Product/

        public ActionResult Index()
        {            
            return View(db.Products.ToList());
        }

        //
        // GET: /Product/Details/5

        public ActionResult Details(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Product/Create
        [Authorize(Users = "vlad")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        //
        // GET: /Product/Edit/5
        [Authorize(Users = "vlad")]
        public ActionResult Edit(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //
        // GET: /Product/Delete/5
        [Authorize(Users = "vlad")]
        public ActionResult Delete(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

      

        #region BUY
        public ActionResult Buy(int id)
        {
            //Console.WriteLine("Product controller - but method");
            
            if (Session["cart"] == null)
            {
                c = new Cart();
                Session["cart"] = c;
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            if ( product.numberOfProducts > 0 )
            {
                var cart = Session["cart"] as Cart;
                cart.addToCart(product);
                Session["cart"] = cart;                
                if (product.numberOfProducts == 1)
                {                   
                    db.Products.Remove(product);
                    db.SaveChanges();                                       
                }
                else
                {
                    product.numberOfProducts--;
                    db.SaveChanges();
                }

                    
            }

          //return RedirectToAction("Index");
            return View(product);

               
        }  

        #endregion

        #region Show Cart


        public ActionResult ShowCart()
        {
            if (Session["cart"] == null)
            {
                c = new Cart();
                Session["cart"] = c;
            }
            var cart = Session["cart"] as Cart;
            //ViewBag.Message = cart.showProductsInCar();
            string message;

            if (cart.products.Count == 0)
            {
                message = "Empty cart";
                ViewBag.Message = "Empty cart";
            }
            else
            {

                message = "\n Product you want to buy:";
                foreach (Product prod in cart.products)
                {
                    message += prod.name + ", " + prod.price + " lei;   \n";
                }

                message += " \n            Total cost: " + cart.calculateTotalaPrice().ToString();


                ViewBag.Message = message;
            }
         
            return View(); 
        }   
        #endregion 


        #region Export to CSV
        public ActionResult ExportCSV()
        {                    

            ExportFactory objExportFactory = new ExportFactory();
            IExporter currentExporter = objExportFactory.getExportType("CSV");

            return View();
        }
        #endregion

        #region Export to JSON
        public void ExportJSON()
        {                    

            ExportFactory objExportFactory = new ExportFactory();
            IExporter currentExporter = objExportFactory.getExportType("JSON");
        
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}