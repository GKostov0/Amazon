using System.Collections.Generic;

namespace AMAZON.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifier(EStat stat);
    }
}