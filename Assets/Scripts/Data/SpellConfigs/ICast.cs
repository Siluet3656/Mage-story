using UnityEngine;

namespace Data.SpellConfigs
{
    public interface ICast
    {
        float GetCastTime();
        float GetCooldown();
        Color GetCastBarColor();
    }
}
