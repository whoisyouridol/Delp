using Delp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delp.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Animal> Animals { get; set; }
    }


    public class Test
    {
        private void TestMethod()
        {
            var context = new ApplicationDbContext();
        }
    }
}
