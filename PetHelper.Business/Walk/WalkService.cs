using AutoMapper;
using PetHelper.Api.Models.RequestModels.Walks;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain.Pets;
using PetHelper.Domain.Properties;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.Business.Walk
{
    public class WalkService : DataAccessableService<WalkModel>, IWalkService
    {
        private readonly IMapper _mapper;

        public WalkService(IMapper mapper, IRepository<WalkModel> repo) : base(repo)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResult<WalkModel>> StartWalk(WalkRequestModel walkRequestModel)
        {
            var walkResult = new ServiceResult<WalkModel>();

            if (walkRequestModel == null)
            {
                walkResult.FailAndThrow(Resources.InvalidDataFound);
            }

            var walkDomainModel = _mapper.Map<WalkModel>(walkRequestModel);
            walkDomainModel.Id = Guid.NewGuid();
            walkDomainModel.StartTime = DateTime.UtcNow;
            walkResult.Value = walkDomainModel;

            var inserResult = await _repository.Insert(walkDomainModel);
            inserResult.CatchAny();

            return walkResult;
        }

        public async Task<ServiceResult<IEnumerable<WalkModel>>> GetWalks(Expression<Func<WalkModel, bool>> predicate)
        {
            var walkResult = new ServiceResult<IEnumerable<WalkModel>>();

            walkResult = await _repository.Where(predicate);
            walkResult.CatchAny();

            return walkResult;
        }

        public async Task<ServiceResult<Empty>> FinishWalk(Guid walkId)
        {
            var serviceResult = new ServiceResult<Empty>();

            if(walkId == Guid.Empty)
            {
                serviceResult.FailAndThrow(Resources.InvalidDataFound);
            }

            var targetWalkResult = await GetWalks(x => x.Id == walkId);
            var targetWalk = targetWalkResult.CatchAny().Value.FirstOrDefault();

            if(targetWalk == null)
            {
                targetWalkResult.FailAndThrow(Resources.TheItemDoesntExist);
            }

            if(targetWalk!.EndTime != null)
            {
                targetWalkResult.FailAndThrow(Resources.TheWalkIsAlreadyFinished);
            }

            targetWalk!.EndTime = DateTime.UtcNow;

            serviceResult = await _repository.Update(targetWalk);
            serviceResult.CatchAny();

            return serviceResult;
        }
    }
}
