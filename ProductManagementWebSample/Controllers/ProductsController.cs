using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using ProductManagementWebSample.Models;
using DataLibrary;
using DataLibrary.JSON_Models;

namespace ProductManagementWebSample.Controllers
{
    public class ProductsController : Controller
    {        
        public ViewResult Index(string dataStorage)
        {
            ViewBag.Title = "Product List";
            ViewBag.Message = "Online shop";

            string selectedDataStorage = GetSelectedDataStorage();

            if (!string.IsNullOrEmpty(dataStorage))
            {
                System.Web.HttpContext.Current.Application["SelectedDataStorage"] = dataStorage;
                selectedDataStorage = dataStorage;
            }

            ViewBag.SelectedDataStorage = selectedDataStorage;

            List<ProductModel> productModels = GetProducts(selectedDataStorage);

            return View("ProductList", productModels);
        }
             
        [System.Web.Mvc.HttpGet]
        public ActionResult Search([FromBody]List<ProductModel> productModels, string selectedDataStorage)
        {
            List<ProductModel> products;

            switch (selectedDataStorage.ToLower())
            {
                case "db":
                    products = GetProducts(selectedDataStorage);
                    break;
                case "memory":
                    products = GetMemoryProductModels();
                    break;
                default:
                    products = new List<ProductModel>();//In case any other value comes an empty list is returned
                    break;
            }

            ViewBag.Title = "Product List";
            ViewBag.Message = "Online shop";
            ViewBag.SelectedDataStorage = selectedDataStorage;

            return View("ProductList", products);
        }

        private List<ProductModel> GetProducts(string selectedDataStorage)
        {
            List<ProductModel> productModels = null;
            switch (selectedDataStorage.ToLower())
            {
                case "db":
                    DataController dataController = new DataController();
                    List<Product> products = dataController.GetProducts();
                    productModels = (from product in products
                                     select new ProductModel()
                                     {
                                         ProductNumber = product.ProductNumber,
                                         Title = product.Title,
                                         Price = product.Price,
                                         DateCreated = product.DateCreated
                                     }).ToList();
                    break;
                case "memory":
                    productModels = GetMemoryProductModels();
                    break;
                default:
                    productModels = new List<ProductModel>();//In case any other value comes an empty list is returned
                    break;
            }

            return productModels;
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
