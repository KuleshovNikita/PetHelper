using Moq;
using NUnit.Framework;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.UnitTests.Utils
{
    public abstract class TestBase<TSut>
    {
        protected abstract TSut GetNewSut();

        protected T Any<T>() => It.IsAny<T>();

        protected Expression<Func<T, bool>> AnyExpression<T>()
            => Any<Expression<Func<T, bool>>>();

        protected Task<ServiceResult<T>> TaskServiceResult<T>(T value)
            => Task.FromResult(new ServiceResult<T> { Value = value }.Success());

        protected void InvokeActAndAssertFailedResult(AsyncTestDelegate action)
            => Assert.ThrowsAsync<FailedServiceResultException>(action);
    }
}
