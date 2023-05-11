using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarSync.Entities
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").AddUserSecrets<Program>(true).Build();
                string connectionString = configuration.GetConnectionString("Database");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LinkedCalendar>().HasKey(lc => new { lc.RootCalendarId, lc.LinkedCalendarId });
        }

        public DbSet<Calendar> Calendars { get; set; }

        public DbSet<ClonedEvent> ClonedEvents { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<MicrosoftUser> MicrosoftUsers { get; set; }

        public DbSet<LinkedCalendar> LinkedCalendars { get; set; }

    }
}
