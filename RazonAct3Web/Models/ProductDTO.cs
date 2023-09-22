using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RazonAct3Web.Models
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int CategoryId { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int Price { get; set; }
    }
}