﻿using PetHelper.Api.Models.RequestModels.Statistic;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain.Exceptions;
using PetHelper.Domain.Pets;
using PetHelper.Domain.Statistic;
using PetHelper.ServiceResulting;

namespace PetHelper.Business.Statistic
{
    public class StatisticService : IStatisticService
    {
        private readonly IRepository<IdlePetStatisticModel> _idleStaticRepository;
        private readonly IRepository<WalkModel> _walkRepository;

        public StatisticService(IRepository<WalkModel> walkRepository, IRepository<IdlePetStatisticModel> idleStaticRepository)
        {
            _idleStaticRepository = idleStaticRepository;
            _walkRepository = walkRepository;
        }

        public async Task<ServiceResult<StatisticModel>> GetStatistic(StatisticRequestModel model)
        {
            var walkResult = await _walkRepository.Where(x => x.StartTime == model.SampleStartDate
                                                           && x.EndTime == model.SampleEndDate
                                                           && x.PetId == model.PetId);  

            var walksData = walkResult.CatchAny().Value;
            var targetPet = walksData.First().Pet;
            var statistic = new StatisticModel
            {
                SampleStartDate = model.SampleStartDate,
                SampleEndDate = model.SampleEndDate
            };

            var walksTimeHistory = walksData.Select(x => (x.StartTime, x.EndTime));

            var filteredWalksHistory = SkipUnfinishedWalks(walksTimeHistory).ToList();

            if (targetPet.AnimalType is null)
            {
                statistic.WalkDuringCriteria.CalculateWithoutIdle(filteredWalksHistory);
                statistic.WalksCountCriteria.CalculateWithoutIdle(filteredWalksHistory);
            }
            else
            {
                statistic.IdlePetStatisticModel = await GetPetIdleStatistic(targetPet);

                statistic.WalkDuringCriteria.Calculate(statistic.IdlePetStatisticModel.IdleWalkDuringTime, filteredWalksHistory);
                statistic.WalksCountCriteria.Calculate(statistic.IdlePetStatisticModel.IdleWalksCountPerDay, filteredWalksHistory);
            }

            return new ServiceResult<StatisticModel>
            {
                Value = statistic
            }.Success();
        }

        private IEnumerable<(DateTime StartTime, DateTime EndTime)> SkipUnfinishedWalks(
            IEnumerable<(DateTime StartTime, DateTime? EndTime)> walksTimeHistory)
        {
            var filteredResult = new List<(DateTime StartTime, DateTime EndTime)>();

            foreach(var pair in walksTimeHistory)
            {
                filteredResult.Add((pair.StartTime, pair.EndTime ?? DateTime.MinValue));
            }

            return filteredResult.Where(x => x.EndTime != DateTime.MinValue);
        }

        private async Task<IdlePetStatisticModel> GetPetIdleStatistic(PetModel targetPet)
        {
            if(string.IsNullOrEmpty(targetPet.Breed))
            {
                return await GetGeneralIdlePetData(targetPet.AnimalType);
            }

            var breedResult = await _idleStaticRepository.FirstOrDefault(x => x.Breed == targetPet.Breed
                                                                           && x.AnimalType == targetPet.AnimalType);

            if (breedResult.Exception is not null && breedResult.Exception is EntityNotFoundException || 
                breedResult.Exception?.InnerException is EntityNotFoundException)
            {
                return await GetGeneralIdlePetData(targetPet.AnimalType);
            }

            return breedResult.CatchAny().Value;
        }

        private async Task<IdlePetStatisticModel> GetGeneralIdlePetData(AnimalType? animalType)
        {
            var result = await _idleStaticRepository.FirstOrDefault(x => x.AnimalType == animalType 
                                                                      && x.IsUnifiedAnimalData == true);
            return result.CatchAny().Value;
        }
    }
}
