using TransformerMonitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TransformerMonitor.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Always ensure we have some transformers
        if (!await context.Transformers.AnyAsync())
        {
            context.Transformers.AddRange(new List<Transformer>
            {
                new() { AssetId = 101, Name = "Main Substation A", Region = "North", BaseVoltage = 22000 },
                new() { AssetId = 202, Name = "Industrial Zone B", Region = "South", BaseVoltage = 18000 },
                new() { AssetId = 303, Name = "Residential Grid C", Region = "East", BaseVoltage = 21000 }
            });
            await context.SaveChangesAsync();
        }

        // Always ensure we have online teams for the showcase
        if (!await context.Teams.AnyAsync())
        {
            var teams = new List<Team>
            {
                new() 
                { 
                    Name = "Rapid Response Team Alpha", 
                    ShiftStart = new TimeSpan(0, 0, 0), 
                    ShiftEnd = new TimeSpan(23, 59, 59), 
                    IsOnShiftToday = true,
                    Technicians = new List<Technician>
                    {
                        new() { Name = "Nikola Tesla", Specialty = "Voltage Expert" },
                        new() { Name = "Michael Faraday", Specialty = "Grid Maintenance" }
                    }
                },
                new() 
                { 
                    Name = "Night Shift Bravo", 
                    ShiftStart = new TimeSpan(22, 0, 0), 
                    ShiftEnd = new TimeSpan(6, 0, 0), 
                    IsOnShiftToday = true,
                    Technicians = new List<Technician>
                    {
                        new() { Name = "Thomas Edison", Specialty = "Electrical Systems" }
                    }
                }
            };

            context.Teams.AddRange(teams);
            await context.SaveChangesAsync();
        }
        else 
        {
            // If teams exist, make sure they are marked as online for the showcase
            var existingTeams = await context.Teams.ToListAsync();
            foreach(var team in existingTeams)
            {
                team.IsOnShiftToday = true;
            }
            await context.SaveChangesAsync();
        }
    }
}
