using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Models;
using DataLibrary.JSON_Models;

namespace DataLibrary
{
    public class DataController
    {
        /// <summary>
        /// Saves a new Product
        /// </summary>
        /// <param name="product">JSON object containing the Product data</param>
        /// <returns>Number of records affected</returns>
        /// <exception cref="InvalidOperationException">Throws an exception when there is already a Product with a given Product ID</exception>
        public int SaveProduct(JSON_Models.Product product)
        {
            //Obtaining the Model to be saved
            Models.Product productModel = new Models.Product()
            {
                ProductNumber = int.Parse(product.ProductNumber),
                Title = product.Title,
                Price = decimal.Parse(product.Price),
                DateCreated = DateTime.Parse(product.DateCreated)
            };

            int records = 0;
            bool productExist = false;

            using (var model = new WebshopEntities())
            {
                productExist = model.Products.Any(p => p.ProductNumber == productModel.ProductNumber);
                if (!productExist)
                {
                    model.Products.Add(productModel);
                    records = model.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException(string.Format("There is already a Product with Product Number: {0}", productModel.ProductNumber));
                }
            }

            return records;
        }

        /// <summary>
        /// Saves a new Order
        /// </summary>
        public void SaveOrder()
        {
            //TODO: When Saving an Order this method should be implemented
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves a new Category
        /// </summary>
        public void SaveCategory()
        {
            //TODO: When Saving a new Category this method should be implemented
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves a new Customer
        /// </summary>
        public void SaveCustomer()
        {
            //TODO: When Saving a new Customer this method should be implemented
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtains the list of all Products
        /// </summary>
        /// <returns>List of JSON objects representing a Product</returns>
        public List<JSON_Models.Product> GetProducts()
        {
            List<JSON_Models.Product> products;
            using (var model = new WebshopEntities())
            {
                products = (from product in model.Products
                            select new JSON_Models.Product()
                            {
                                ProductNumber = product.ProductNumber.ToString(),
                                Title = product.Title,
                                Price = product.Price.ToString(),
                                DateCreated = product.DateCreated.ToString()
                            }).ToList();
            }
            return products;
        }
    }
}
