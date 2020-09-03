using System.Collections.Generic;

namespace RPG.Stats{
    interface IModifierProvider
    {
        IEnumerable<float> GetAdditionalModifier(Stats stats);
        IEnumerable<float> GetPercentageModifier(Stats stats);
    }
}
