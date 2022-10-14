using Microsoft.EntityFrameworkCore;

namespace PetHelper.DataAccess.Context.ModelContexts
{
    public interface IModelContext
    {
        public void Configure(ModelBuilder modelBuilder);
    }
}
