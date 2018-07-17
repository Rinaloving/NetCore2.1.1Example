using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreDemo2.Models
{
    public class HelloWorldDBContext : IdentityDbContext<User>
    {
        public HelloWorldDBContext() { }


        public HelloWorldDBContext(DbContextOptions<HelloWorldDBContext> options)
            :base(options)
        {
            Database.EnsureCreated(); //必须有，否则SQLite数据迁移不生成数据
        }

        public DbSet<Employee> Employees { get; set; }


    }
}
