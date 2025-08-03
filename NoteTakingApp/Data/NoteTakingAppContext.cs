using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Models;

namespace NoteTakingApp.Data
{
    public class NoteTakingAppContext : DbContext
    {
        public NoteTakingAppContext (DbContextOptions<NoteTakingAppContext> options)
            : base(options)
        {
        }

        public DbSet<NoteTakingApp.Models.Note> Note { get; set; } = default!;
    }
}
