using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.FundooContext
{
    public class FundooDBContext : DbContext
    {
        public FundooDBContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}