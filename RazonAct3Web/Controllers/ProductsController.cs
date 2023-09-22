using RazonAct3Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace RazonAct3Web.Controllers
{

    public class ProductsController : ApiController
    {
        private RazonAct3Entities db = new RazonAct3Entities();

        // GET: api/Products
        [HttpGet]
        [Route("api/products/search")]
        public IHttpActionResult SearchProducts([FromUri] string category = null, [FromUri] string productName = null, [FromUri] string description = null, [FromUri] string color = null)
        {
            try
            {
                var query = db.Product.AsQueryable();

                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(p => p.Category.CategoryCode.Contains(category));
                }

                if (!string.IsNullOrEmpty(productName))
                {
                    query = query.Where(p => p.ProductName.Contains(productName));
                }

                if (!string.IsNullOrEmpty(description))
                {
                    query = query.Where(p => p.ProductDescription.Contains(description));
                }

                if (!string.IsNullOrEmpty(color))
                {
                    query = query.Where(p => p.Color.Contains(color));
                }

                var products = query.ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Products/5
        [HttpGet]
        [Route("api/Products/Find/{productCode}")]
        public IHttpActionResult GetProductByCode(string productCode)
        {
            // Find the product by its product code
            var product = db.Product.SingleOrDefault(p => p.ProductCode == productCode);

            if (product == null)
            {
                return NotFound(); // Product not found
            }

            // Map the product entity to a DTO
            var productDTO = new ProductDTO
            {
                ProductId = product.ProductId,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                CategoryId = product.CategoryId,
                Color = product.Color,
                Size = product.Size,
                Price = product.Price,
            };

            return Ok(productDTO);
        }

        // POST: api/Products
        [HttpPost]
        [Route("{categoryId:int}/addproduct")]
        public IHttpActionResult AddProductToCategory(int categoryId, Product productinfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var category = db.Category.Find(categoryId);
                if (category == null)
                {
                    return NotFound();
                }

                var product = new Product
                {
                    ProductCode = productinfo.ProductCode,
                    ProductName = productinfo.ProductName,
                    ProductDescription = productinfo.ProductDescription,
                    Price = productinfo.Price,
                    Color = productinfo.Color,
                    Size = productinfo.Size,
                    CategoryId = categoryId
                };

                category.Product.Add(product);
                db.SaveChanges();
                return Ok("Product added to category successfully.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPut]
        [Route("api/products/{productCode}")]
        public IHttpActionResult UpdateProductByCode(string productCode, [FromBody] ProductDTO updatedProductDTO)
        {
            try
            {
                // Find the product by its product code
                var product = db.Product.SingleOrDefault(p => p.ProductCode == productCode);

                if (product == null)
                {
                    return NotFound(); // Product not found
                }

                // Update the product properties based on the updatedProductDTO
                product.ProductName = updatedProductDTO.ProductName;
                product.ProductDescription = updatedProductDTO.ProductDescription;
                product.CategoryId = updatedProductDTO.CategoryId;
                product.Color = updatedProductDTO.Color;
                product.Size = updatedProductDTO.Size;
                product.Price = updatedProductDTO.Price;

                // Save changes to the database
                db.SaveChanges();

                return Ok("Product updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                // Handle validation errors if any
                var validationErrors = db.GetValidationErrors();

                foreach (var entityValidationErrors in validationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Validation Error: {validationError.ErrorMessage}");
                    }
                }

                return BadRequest("Validation error. Please check the data.");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                return InternalServerError(ex);
            }
        }

        // DELETE: api/Products/5
        [HttpDelete]
        [Route("api/products/{productCode}")]
        public IHttpActionResult DeleteProductByCode(string productCode)
        {
            try
            {
                var product = db.Product.SingleOrDefault(p => p.ProductCode == productCode);
                if (product == null)
                {
                    return NotFound();
                }

                db.Product.Remove(product);
                db.SaveChanges();
                return Ok("Product deleted successfully");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


    }
}