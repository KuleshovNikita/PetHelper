using PetHelper.ServiceResulting;

namespace PetHelper.UnitTests.Utils
{
    internal static class ResultUtils<T>
    {
        public static ServiceResult<T> Success(T value)
            => new ServiceResult<T>
            {
                Value = value,
            }.Success();
    }
}
