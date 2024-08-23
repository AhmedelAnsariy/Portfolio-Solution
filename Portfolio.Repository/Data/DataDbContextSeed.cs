using Portfolio.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Portfolio.Repository.Data
{
    public static  class DataDbContextSeed
    {
        public static async Task SeedAsync(DataDbContext _context)
        {
            if(_context.ClientReviews.Count() == 0)
            {
                var clients = File.ReadAllText("../Portfolio.Repository/Data/DataSeed/clients.json");
                var clientsData = JsonSerializer.Deserialize<List<ClientReview>>(clients);

                if (clientsData?.Count > 0)
                {
                    foreach (var client in clientsData)
                    {
                        _context.Set<ClientReview>().Add(client);
                    }
                    await _context.SaveChangesAsync();
                }
            }


            if (_context.Categories.Count() == 0)
            {
                var categories = File.ReadAllText("../Portfolio.Repository/Data/DataSeed/category.json");


                var categoriesData = JsonSerializer.Deserialize<List<Category>>(categories);

                if (categoriesData?.Count > 0)
                {
                    foreach (var cat in categoriesData)
                    {
                        _context.Set<Category>().Add(cat);
                    }
                    await _context.SaveChangesAsync();
                }
            }






            if (_context.Designs.Count() == 0)
            {
                var designs = File.ReadAllText("../Portfolio.Repository/Data/DataSeed/designs.json");




                var designsData = JsonSerializer.Deserialize<List<Design>>(designs);




                if (designsData?.Count > 0)
                {
                    foreach (var  des in designsData)
                    {
                        _context.Set<Design>().Add(des);
                    }
                    await _context.SaveChangesAsync();
                }
            }



            if(_context.Careers.Count() == 0)
            {
                var careers = File.ReadAllText("../Portfolio.Repository/Data/DataSeed/career.json");
                var careerData = JsonSerializer.Deserialize<List<Career>>(careers);




                if (careerData?.Count > 0)
                {
                    foreach (var car in careerData)
                    {
                        _context.Set<Career>().Add(car);
                    }
                    await _context.SaveChangesAsync();
                }
            }







        }
    }
}
