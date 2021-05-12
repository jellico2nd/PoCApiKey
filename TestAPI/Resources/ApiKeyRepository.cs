using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPI.Resources
{
    public class ApiKeyRepository : DbContext
    {
        public ApiKeyRepository(DbContextOptions<ApiKeyRepository> options) : base (options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApiKeyEntity>().HasData(new ApiKeyEntity { Id = 1, ApiKeyValue = "yes" });
        }

        public DbSet<ApiKeyEntity> ApiKeys { get; set; }
    }
}
