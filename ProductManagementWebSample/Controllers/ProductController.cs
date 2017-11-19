using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataLibrary;
using DataLibrary.JSON_Models;
using ProductManagementWebSample.Models;
using System.Web.Mvc;

namespace ProductManagementWebSample.Controllers
{
    public class ProductController : Controller
    {
        private string copyright = "Online shop";

        public ViewResult Index()
        {
            ViewBag.Title = "Create Product";
            ViewBag.Message = this.copyright;
            ViewBag.SelectedDataStorage = GetSelectedDataStorage();
            return View("Product");
        }
                
        [System.Web.Mvc.HttpPost]
        public ActionResult CreateProduct([FromBody]ProductModel model, string selectedDataStorage)
        {
            try
            {
                int records = 0;
                System.Web.HttpContext.Current.Application["SelectedDataStorage"] = selectedDataStorage;

                switch (selectedDataStorage.ToLower())
                {
                    case "db":
                        DataController controller = new DataController();
                        Product jsonProduct = new Product()
                        {
                            Title = model.Title,
                            ProductNumber = model.ProductNumber,
                            Price = model.Price,
                            DateCreated = model.DateCreated == null ? DateTime.UtcNow.ToString() : model.DateCreated
                        };                        
                        records = controller.SaveProduct(jsonProduct);
                        break;
                    case "memory":
                        System.Web.HttpContext.Current.Application.Lock();
                        List<ProductModel> products = GetMemoryProductModels();                            

                        if (!products.Any(p => p.ProductNumber == model.ProductNumber))
                        {
                            products.Add(model);
                            records++;
                        }
                        else {
                            throw new InvalidOperationException(string.Format("There is already a Product with Product Number: {0}", model.ProductNumber));
                        }
                        
                        System.Web.HttpContext.Current.Application["ProductModels"] = products;
                        System.Web.HttpContext.Current.Application.UnLock();
                        break;
                    default:
                        break;
                }

                ViewBag.SaveResultMessage = records > 0 ? "Your product has been successfully saved." : string.Empty;
            }
            catch (InvalidOperationException e)
            {
                //Handling of an InvalidOperationException, for example a duplicate key when saving a product
                ViewBag.InvalidOperationMessage = e.Message;
            }
            catch (Exception)
            {
                //Handle any exception here
                ViewBag.ErrorResultMessage = "An error has occurred when trying to save a new product.";
            }
            finally
            {
                ViewBag.Message = this.copyright;
                ViewBag.SelectedDataStorage = GetSelectedDataStorage();
                //Clearing the model so another product can be added if needed
                ModelState.Clear();
            }

            return View("Product");
        }

        private List<ProductModel> GetMemoryProductModels()
        {
            return System.Web.HttpContext.Current.Application["ProductModels"] == null ?
                        new List<ProductModel>() :
                        System.Web.HttpContext.Current.Application["ProductModels"] as List<ProductModel>;
        }

        private string GetSelectedDataStorage()
        {
            return System.Web.HttpContext.Current.Application["SelectedDataStorage"] != null ?
                                System.Web.HttpContext.Current.Application["SelectedDataStorage"] as string :
                                "db";
        }
    }
}
