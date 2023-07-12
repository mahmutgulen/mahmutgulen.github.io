using FranchiseMenu.CORE.Entities.Concrete;
using FranchiseMenu.ENTITY.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FranchiseMenu.DAL.Concrete.EntityFramework
{
    public class FranchiseMenuContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=intern-db.cjq6i1xxy6zz.eu-central-1.rds.amazonaws.com;Database=FranchiseMenuProjectMako;Uid=sa;Password=ntKjHbkxnkdEDRYbEGgdnXGZ;TrustServerCertificate=True");
            //get shit done
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SecurityHistory> SecurityHistories { get; set; }
    }
}
