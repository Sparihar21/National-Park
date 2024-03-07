﻿using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        
        }
        public DbSet<NationalPark> NationalParks { get; set; }
        public DbSet<Trails> Trails { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
