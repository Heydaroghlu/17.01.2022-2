﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Models
{
   
        public class DataContext : IdentityDbContext
        {
            public DataContext(DbContextOptions<DataContext> options) : base(options)
            {

            }

            public DbSet<Slider> Sliders { get; set; }
            public DbSet<Feature> Features { get; set; }
            public DbSet<Author> Authors { get; set; }
            public DbSet<Genre> Genres { get; set; }
            public DbSet<Book> Books { get; set; }
            public DbSet<BookImage> BookImages { get; set; }
            public DbSet<Tag> Tags { get; set; }
            public DbSet<BookTag> BookTags { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<BookComment> BookComments { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        }
    }
