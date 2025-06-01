using System.Collections.Generic;

namespace AMAZON.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifiers(EStat stat);
        IEnumerable<float> GetPercentageModifiers(EStat stat);
    }
}