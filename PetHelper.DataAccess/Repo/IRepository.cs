﻿using PetHelper.Domain;
using System.Linq.Expressions;

namespace PetHelper.DataAccess.Repo
{
    public interface IRepository<T> where T : class
    {
        Task<T> Single(Expression<Func<T, bool>> command);

        Task<bool> Any(Expression<Func<T, bool>> command);

        Task Insert(T entity);
    }
}
