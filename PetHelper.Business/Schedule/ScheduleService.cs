using AutoMapper;
using PetHelper.Api.Models.RequestModels.Schedules;
using PetHelper.Business.Extensions;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain.Exceptions;
using PetHelper.Domain.Pets;
using PetHelper.Domain.Properties;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.Business.Schedule
{
    public class ScheduleService : DataAccessableService<ScheduleModel>, IScheduleService
    {
        private readonly IMapper _mapper;

        public ScheduleService(IMapper mapper, IRepository<ScheduleModel> repo) 
            : base(repo) => _mapper = mapper;

        public async Task<ServiceResult<Empty>> AddSchedule(ScheduleRequestModel scheduleRequestModel)
        {
            var serviceResult = new ServiceResult<Empty>();

            if (scheduleRequestModel == null)
            {
                return serviceResult.FailAndThrow(Resources.InvalidDataFound);
            }

            var scheduleDomainModel = _mapper.Map<ScheduleModel>(scheduleRequestModel);

            var result = await _repository.Insert(scheduleDomainModel);
            result.CatchAny();

            return serviceResult.Success();
        }

        public async Task<ServiceResult<ScheduleModel>> GetSchedule(Expression<Func<ScheduleModel, bool>> predicate)
        {
            var result = await _repository.FirstOrDefault(predicate);
            return result.Catch<EntityNotFoundException>(Resources.TheItemDoesntExist)
                         .CatchAny();
        }

        public async Task<ServiceResult<IEnumerable<ScheduleModel>>> GetSchedules(Expression<Func<ScheduleModel, bool>> predicate)
        {
            var result = await _repository.Where(predicate);
            return result.CatchAny();
        }

        public async Task<ServiceResult<Empty>> UpdateSchedule(ScheduleUpdateRequestModel scheduleUpdateModel, Guid scheduleId)
        {
            var scheduleModel = await GetSchedule(x => x.Id == scheduleId);
            scheduleModel.Value = _mapper.MapOnlyUpdatedProperties(scheduleUpdateModel, scheduleModel.Value);

            var result = await _repository.Update(scheduleModel.Value);
            return result.CatchAny();
        }

        public async Task<ServiceResult<Empty>> RemoveSchedule(Guid scheduleId)
        {
            var serviceResult = new ServiceResult<Empty>();

            if (await ScheduleExists(scheduleId))
            {
                var scheduleModel = await GetSchedule(x => x.Id == scheduleId);
                var removeResult = await _repository.Remove(scheduleModel.Value);
                return removeResult.CatchAny();
            }

            return serviceResult.FailAndThrow(Resources.TheItemDoesntExist);
        }

        private async Task<bool> ScheduleExists(Guid scheduleId)
        {
            var result = await _repository.Any(x => x.Id == scheduleId);
            return result.CatchAny().Value;
        }
    }
}
