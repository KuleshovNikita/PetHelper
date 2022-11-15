using System.ComponentModel;

namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public enum CriteriaResult
    {
        [Description("Good")]
        Good = 0,

        [Description("Acceptable")]
        Acceptable = 1,

        [Description("Bad")]
        Bad = 2,

        [Description("Very Bad")]
        VeryBad = 3,
    }
}
