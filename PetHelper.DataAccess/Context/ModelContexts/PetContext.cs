using Microsoft.EntityFrameworkCore;
using PetHelper.Domain.Pets;

namespace PetHelper.DataAccess.Context.ModelContexts
{
    public class PetContext : IModelContext
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PetModel>().HasKey(x => x.Id);
            modelBuilder.Entity<PetModel>(entity =>
            {
                entity.Property(x => x.Id)
                      .IsRequired();

                entity.Property(x => x.Name)
                      .HasMaxLength(30)
                      .IsRequired();

                entity.Property(x => x.AnimalType)
                      .IsRequired();

                entity.Property(x => x.Breed)
                      .HasMaxLength(50)
                      .IsRequired();
            });
        }
    }
}
