using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AnimeProject.Models;

namespace AnimeProject
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            
        }
    }
}