using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covaci_Adriana_Laborator2.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }


        public int? AuthorID { get; set; }

        public Author? Author { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int? GenreID { get; set; }
        public Genre? Genre { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
