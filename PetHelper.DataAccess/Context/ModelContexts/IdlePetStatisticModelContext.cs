using Microsoft.EntityFrameworkCore;
using PetHelper.Domain.Statistic;

namespace PetHelper.DataAccess.Context.ModelContexts
{
    public class IdlePetStatisticModelContext : IModelContext
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdlePetStatisticModel>().HasKey(x => x.Id);
            modelBuilder.Entity<IdlePetStatisticModel>(entity =>
            {
                entity.Property(x => x.Id)
                      .IsRequired();

                entity.Property(x => x.AnimalType)
                      .IsRequired();

                entity.Property(x => x.IdleWalksCountPerDay)
                      .IsRequired();

                entity.Property(x => x.IdleWalksCountPerDay)
                      .IsRequired();

                entity.Property(x => x.IsUnifiedAnimalData)
                      .IsRequired();

                entity.Property(x => x.Breed)
                      .IsRequired(false)
                      .HasMaxLength(50);
            });
        }
    }
}
