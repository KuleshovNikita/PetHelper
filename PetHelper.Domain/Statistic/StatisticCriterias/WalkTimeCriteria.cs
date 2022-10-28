using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public record WalkTimeCriteria : BaseStatCriteria
    {
        public double AverageWalkTimesPerDay { get; set; }

        public double IdleWalkTimesPerDay { get; set; }
    }
}
