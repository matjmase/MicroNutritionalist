using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroNutritionalist.Db.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed(Unique = true)]
        public string Name { get; set; }
        public int Calories { get; set; }
        public string ServingDescription { get; set; }
    }
}
