using Microsoft.EntityFrameworkCore;
using PetHelper.Domain.Pets;

namespace PetHelper.DataAccess.Context.ModelContexts
{
    public class ScheduleContext : IModelContext
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScheduleModel>().HasKey(x => x.Id);
            modelBuilder.Entity<ScheduleModel>(entity =>
            {
                entity.Property(x => x.Id)
                      .IsRequired();

                entity.Property(x => x.ScheduledStart)
                      .IsRequired();

                entity.Property(x => x.ScheduledEnd)
                      .IsRequired();

                entity.HasOne(x => x.Pet)
                      .WithMany(x => x.WalkingSchedule)
                      .HasForeignKey(x => x.PetId)
                      .IsRequired();
            });
        }
    }
}
