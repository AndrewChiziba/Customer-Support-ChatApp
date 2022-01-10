using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using HelloFuture.Models;

namespace HelloFuture.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        

        public virtual DbSet<Person> People { get; set; }
        //public virtual DbSet<Header> Headers { get; set; }
        //public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<CallAgent> CallAgents { get; set; }

    }
}
