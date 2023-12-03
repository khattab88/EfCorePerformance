using FormulaOne.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaOne.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Achievement> Achievements { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.HasOne<Driver>(a => a.Driver)
                    .WithMany(d => d.Achievements)
                    .HasForeignKey(a => a.DriverId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Achievements_Driver");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasMany<Ticket>(e => e.Tickets)
                .WithOne()
                .HasForeignKey(t => t.EventId)
                .IsRequired();

                var demoEvent = new Event()
                {
                    Id = 1,
                    Location = "Silverstone",
                    Name = "British Brexit"
                };

                entity.HasData(demoEvent);
            });

            modelBuilder.Entity<Ticket>(entity => 
            {
                var tickets = Enumerable
                    .Range(0, 5000)
                    .Select(id => new Ticket() 
                    {
                        Id = Guid.NewGuid(),
                        EventDate = DateTime.UtcNow.AddDays(10),
                        Price = 100,
                        TicketLevel = "Bronze",
                        EventId = 1,
                        Status = 1,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    });

                entity.HasData(tickets);
            });
        }
    }
}
