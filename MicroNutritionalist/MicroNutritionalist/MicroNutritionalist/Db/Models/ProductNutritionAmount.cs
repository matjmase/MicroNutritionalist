using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroNutritionalist.Db.Models
{
    public class ProductNutritionAmount
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed(Unique = false)]
        public int ProductId { get; set; }
        [Indexed(Unique = false)]
        public int NutritionId { get; set; }
        public int AmountMg { get; set; }
    }
}
