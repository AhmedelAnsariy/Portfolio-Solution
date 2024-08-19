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
        }
    }
}
