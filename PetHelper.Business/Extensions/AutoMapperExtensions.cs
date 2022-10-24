using AutoMapper;

namespace PetHelper.Business.Extensions
{
    public static class AutoMapperExtensions
    {
        public static TDest MapOnlyUpdatedProperties<TFrom, TDest>(this IMapper mapper, TFrom from, TDest to)
            where TFrom : class
            where TDest : class
        {
            var propertiesToUpdate = from.GetType()
                                         .GetProperties()
                                         .Where(x => x.GetValue(from) != null)
                                         .ToList();

            var userModelProps = to.GetType().GetProperties();

            foreach (var prop in propertiesToUpdate)
            {
                var propToUpdate = userModelProps.First(x => x.Name == prop.Name);
                propToUpdate.SetValue(to, prop.GetValue(from));
            }

            return to;
        }
    }
}
