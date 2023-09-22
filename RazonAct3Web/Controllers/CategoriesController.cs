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
    public class CategoriesController : ApiController
    {
        private RazonAct3Entities db = new RazonAct3Entities();

        // GET: api/Categories
        public IQueryable<Category> GetCategory()
        {
            return db.Category.Include(c => c.Product);
        }

        // GET: api/Categories/5
        [HttpGet]
        [Route("api/products/bycategory/{categoryCode}")]
        public IHttpActionResult GetProductsByCategoryCode(string categoryCode)
        {
            try
            {
                var category = db.Category.SingleOrDefault(c => c.CategoryCode == categoryCode);
                if (category != null)
                {
                    return NotFound();
                }

                var products = db.Product.Where(p => p.CategoryId == category.CategoryId).ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Category.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Category.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        private bool CategoryExists(int id)
        {
            return db.Category.Count(e => e.CategoryId == id) > 0;
        }

    }
}