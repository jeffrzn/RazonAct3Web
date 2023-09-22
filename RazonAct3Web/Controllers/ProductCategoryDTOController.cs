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
    [RoutePrefix("api/productcategory")]
    public class ProductCategoriesDTOController : ApiController
    {
        private RazonAct3Entities db = new RazonAct3Entities();

        // Post: ProductCategoriesDTO
        [Route("create")]
        public IHttpActionResult Post(ProductCategoryDTO productCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Create Product and Category entities based on the DTO
                var product = new Product
                {
                    ProductCode = productCategoryDto.ProductCode,
                    ProductName = productCategoryDto.ProductName,
                    ProductDescription = productCategoryDto.ProductDescription,
                    CategoryId = productCategoryDto.CategoryId,
                    Color = productCategoryDto.Color,
                    Size = productCategoryDto.Size,
                    Price = productCategoryDto.Price,

                };

                var category = new Category
                {
                    CategoryCode = productCategoryDto.CategoryCode,
                    CategoryDescription = productCategoryDto.CategoryDescription
                };

                // Add the entities to the DbContext and save changes
                db.Product.Add(product);
                db.Category.Add(category);
                db.SaveChanges();

                var response = new
                {
                    ProductId = product.ProductId,
                    CategoryId = category.CategoryId,
                    Message = "Product and Category created successfully."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an appropriate response
                return InternalServerError(ex);
            }
        }


    }
}