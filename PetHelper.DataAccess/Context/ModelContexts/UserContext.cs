using Microsoft.EntityFrameworkCore;
using PetHelper.Domain;

namespace PetHelper.DataAccess.Context.ModelContexts
{
    public class UserContext : IModelContext
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasKey(x => x.Id);
            modelBuilder.Entity<UserModel>().HasAlternateKey(x => x.Login);
            modelBuilder.Entity<UserModel>().HasAlternateKey(x => x.Password);
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.Property(x => x.Id)
                      .IsRequired();

                entity.Property(x => x.Age);

                entity.Property(x => x.FirstName)
                      .HasMaxLength(30)
                      .IsRequired();

                entity.Property(x => x.LastName)
                      .HasMaxLength(30)
                      .IsRequired();

                entity.HasMany(x => x.Pets)
                      .WithOne(x => x.Owner)
                      .HasForeignKey(x => x.OwnerId);

                entity.Property(x => x.Login)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(x => x.Password)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(x => x.IsEmailConfirmed)
                      .HasDefaultValue(false)
                      .IsRequired();
            });
        }
    }
}
