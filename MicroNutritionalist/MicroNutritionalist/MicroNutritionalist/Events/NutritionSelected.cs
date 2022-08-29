using MicroNutritionalist.Db.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroNutritionalist.Events
{
    public class NutritionSelected : PubSubEvent<Nutrition>
    {
    }
}
