using Microsoft.EntityFrameworkCore;
using PetHelper.Domain.Pets;

namespace PetHelper.DataAccess.Context.ModelContexts
{
    public class WalkHistoryContext : IModelContext
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WalkModel>().HasKey(x => x.Id);
            modelBuilder.Entity<WalkModel>(entity =>
            {
                entity.Property(x => x.Id)
                      .IsRequired();

                entity.Property(x => x.StartTime)
                      .IsRequired();

                entity.Property(x => x.EndTime)
                      .IsRequired(false);

                entity.Property(x => x.ScheduleId)
                      .IsRequired();
                entity.HasOne(x => x.Schedule)
                      .WithMany();

                entity.Property(x => x.PetId)
                      .IsRequired();
            });
        }
    }
}
