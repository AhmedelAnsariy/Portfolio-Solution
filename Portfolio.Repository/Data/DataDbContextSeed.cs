﻿using Microsoft.AspNetCore.Identity;
using Portfolio.Core.Identity;
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
        public static async Task SeedAsync(DataDbContext _context , UserManager<AppUser> _userManager)
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




            if (_context.TeamMembers.Count() == 0)
            {
                var team = File.ReadAllText("../Portfolio.Repository/Data/DataSeed/team.json");
                var TeamData = JsonSerializer.Deserialize<List<TeamMember>>(team);

                if (TeamData?.Count > 0)
                {
                    foreach (var teammemebr in TeamData)
                    {
                        _context.Set<TeamMember>().Add(teammemebr);
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
                    foreach (var des in designsData)
                    {
                        _context.Set<Design>().Add(des);
                    }
                    await _context.SaveChangesAsync();
                }
            }


            if (_context.Sponsprs.Count() == 0)
            {
                var sponsors = File.ReadAllText("../Portfolio.Repository/Data/DataSeed/sponsor.json");

                var sponsorsData = JsonSerializer.Deserialize<List<Sponspr>>(sponsors);


                if (sponsorsData?.Count > 0)
                {
                    foreach (var spon in sponsorsData)
                    {
                        _context.Set<Sponspr>().Add(spon);
                    }
                    await _context.SaveChangesAsync();
                }
            }









            if (_context.Contacts.Count() == 0)
            {
                var contacts = File.ReadAllText("../Portfolio.Repository/Data/DataSeed/contact.json");
                var contactsData = JsonSerializer.Deserialize<List<Contact>>(contacts);




                if (contactsData?.Count > 0)
                {
                    foreach (var cont in contactsData)
                    {
                        _context.Set<Contact>().Add(cont);
                    }
                    await _context.SaveChangesAsync();
                }
            }

            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    FName = "Mohame",
                    Email = "MohameKamal@adminDKK.com",
                    UserName = "MohameKamal",
                    PhoneNumber = "01065351945"
                   
                };

                await _userManager.CreateAsync(user, "Mohamed_Kamal@12345!@#");
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            



        }
    }
}
