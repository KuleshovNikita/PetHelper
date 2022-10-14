﻿using Microsoft.EntityFrameworkCore;
using PetHelper.DataAccess.Context.ModelContexts;
using PetHelper.Domain;
using PetHelper.Domain.Pets;

namespace PetHelper.DataAccess.Context
{
    public class PetHelperDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        public DbSet<PetModel> Pets { get; set; }

        public DbSet<ScheduleModel> WalkingSchedules { get; set; }

        private readonly IReadOnlyCollection<IModelContext> _modelContexts = new List<IModelContext>
        {
            new UserContext(),
            new PetContext(),
            new ScheduleContext(),
        };

        public PetHelperDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var modelContext in _modelContexts)
            {
                modelContext.Configure(modelBuilder);
            }
        }
    }
}
