using MicroNutritionalist.Db.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroNutritionalist.Db
{
    public class NutritionalistDatabase
    {
        private readonly SQLiteAsyncConnection database;

        public NutritionalistDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Product>().Wait();
            database.CreateTableAsync<Nutrition>().Wait();
            database.CreateTableAsync<ProductNutritionAmount>().Wait();
            database.CreateTableAsync<ConsumptionEvent>().Wait();
        }

        #region GetAll

        public Task<List<Product>> GetAllProducts()
        {
            return database.Table<Product>().ToListAsync();
        }

        public Task<List<Nutrition>> GetAllNutrition()
        {
            return database.Table<Nutrition>().ToListAsync();
        }

        public Task<List<ProductNutritionAmount>> GetAllProductNutritionAmount()
        {
            return database.Table<ProductNutritionAmount>().ToListAsync();
        }

        public Task<List<ProductNutritionAmount>> GetAllProductNutritionAmountByProduct(int productId)
        {
            return database.Table<ProductNutritionAmount>().Where(e => e.ProductId == productId).ToListAsync();
        }

        public Task<List<ProductNutritionAmount>> GetAllProductNutritionAmountByNutrition(int nutritionId)
        {
            return database.Table<ProductNutritionAmount>().Where(e => e.NutritionId == nutritionId).ToListAsync();
        }

        public Task<List<ConsumptionEvent>> GetAllConsumptionEvent()
        {
            return database.Table<ConsumptionEvent>().ToListAsync();
        }

        public Task<List<ConsumptionEvent>> GetAllConsumptionEventByTimeWindow(DateTime start, DateTime end)
        {
            return database.Table<ConsumptionEvent>().Where(e => e.Time >= start && e.Time <= end).ToListAsync();
        }
        public Task<List<ConsumptionEvent>> GetAllConsumptionEventByProductId(int productId)
        {
            return database.Table<ConsumptionEvent>().Where(e => e.ProductId == productId).ToListAsync();
        }

        #endregion

        #region ProductCrud

        public Task<Product> GetProduct(int id)
        {
            return database.Table<Product>().FirstOrDefaultAsync(e => e.Id == id);
        }
        public Task<Product> GetProductByName(string name)
        {
            return database.Table<Product>().FirstOrDefaultAsync(e => e.Name == name);
        }

        public Task InsertProduct(Product input)
        {
            return database.InsertAsync(input);
        }

        public Task UpdateProduct(Product input)
        {
            return database.UpdateAsync(input);
        }

        public Task DeleteProduct(Product input)
        {
            return database.DeleteAsync(input);
        }

        public async Task DeleteProductCascade(Product input)
        {
            var prodNutAmt = await GetAllProductNutritionAmountByProduct(input.Id);

            foreach (var item in prodNutAmt)
                await database.DeleteAsync(item);

            var consumeEvents = await GetAllConsumptionEventByProductId(input.Id);

            foreach (var item in consumeEvents)
                await database.DeleteAsync(item);

            await database.DeleteAsync(input);
        }

        #endregion



        #region NutritionCrud

        public Task<Nutrition> GetNutrition(int id)
        {
            return database.Table<Nutrition>().FirstOrDefaultAsync(e => e.Id == id);
        }
        public Task<Nutrition> GetNutrition(string name)
        {
            return database.Table<Nutrition>().FirstOrDefaultAsync(e => e.Name == name);
        }

        public Task InsertNutrition(Nutrition input)
        {
            return database.InsertAsync(input);
        }

        public Task UpdateNutrition(Nutrition input)
        {
            return database.UpdateAsync(input);
        }

        public Task DeleteNutrition(Nutrition input)
        {
            return database.DeleteAsync(input);
        }

        public async Task DeleteNutritionCascade(Nutrition input)
        {
            var prodNutAmt = await GetAllProductNutritionAmountByNutrition(input.Id);

            foreach (var item in prodNutAmt)
                await database.DeleteAsync(item);

            await database.DeleteAsync(input);
        }

        #endregion


        #region ProductNutritionAmountCrud

        public Task<ProductNutritionAmount> GetProductNutritionAmount(int id)
        {
            return database.Table<ProductNutritionAmount>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task InsertProductNutritionAmount(ProductNutritionAmount input)
        {
            return database.InsertAsync(input);
        }

        public Task UpdateProductNutritionAmount(ProductNutritionAmount input)
        {
            return database.UpdateAsync(input);
        }

        public Task DeleteProductNutritionAmount(ProductNutritionAmount input)
        {
            return database.DeleteAsync(input);
        }

        #endregion


        #region ConsumptionEventCrud

        public Task<ConsumptionEvent> GetConsumptionEvent(int id)
        {
            return database.Table<ConsumptionEvent>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task InsertConsumptionEvent(ConsumptionEvent input)
        {
            return database.InsertAsync(input);
        }

        public Task UpdateConsumptionEvent(ConsumptionEvent input)
        {
            return database.UpdateAsync(input);
        }

        public Task DeleteConsumptionEvent(ConsumptionEvent input)
        {
            return database.DeleteAsync(input);
        }

        #endregion
    }
}
