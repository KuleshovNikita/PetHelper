using Microsoft.EntityFrameworkCore;
using PetHelper.Domain;

namespace PetHelper.DataAccess.Context.ModelContexts
{
    public class UserContext : IModelContext
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasKey(x => x.Id);
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.Property(x => x.Id)
                      .IsRequired();

                entity.Property(x => x.Age)
                      .IsRequired();

                entity.Property(x => x.FirstName)
                      .HasMaxLength(30)
                      .IsRequired();

                entity.Property(x => x.LastName)
                      .HasMaxLength(30)
                      .IsRequired();

                entity.HasMany(x => x.Pets)
                      .WithOne(x => x.Owner)
                      .HasForeignKey(x => x.OwnerId)
                      .IsRequired();
            });
        }
    }
}
