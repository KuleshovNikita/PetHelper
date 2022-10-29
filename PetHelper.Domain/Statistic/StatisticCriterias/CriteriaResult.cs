using System.ComponentModel;

namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public enum CriteriaResult
    {
        [Description("Very Bad")]
        VeryBad = 0,

        [Description("Bad")]
        Bad = 1,

        [Description("Acceptable")]
        Acceptable = 2,

        [Description("Good")]
        Good = 3,
    }
}
