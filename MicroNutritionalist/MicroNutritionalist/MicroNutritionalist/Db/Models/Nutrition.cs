using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroNutritionalist.Db.Models
{
    public class Nutrition
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed(Unique = true)]
        public string Name { get; set; }
    }
}
