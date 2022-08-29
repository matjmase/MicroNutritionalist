using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroNutritionalist.Db.Models
{
    public class ConsumptionEvent
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed(Unique = false)]
        public int ProductId { get; set; }
        [Indexed(Unique = false)]
        public DateTime Time { get; set; }
        public double Proportion { get; set; }
    }
}
