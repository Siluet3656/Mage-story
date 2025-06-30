using UnityEngine;

namespace Spells
{
    public interface ICast
    {
        float GetCastTime();
        float GetCooldown();
        Color GetCastBarColor();
    }
}
