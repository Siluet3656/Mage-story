using UnityEngine;
using Data.Enums;

namespace Statuses
{
    public interface IStatusEffect
    {
        StatusType Type { get; }
        StatusCategory Category { get; }
        float Duration { get; }
        bool IsActive { get; }
    
        void Apply(GameObject target);
        void Remove(GameObject target);
        void Update(float deltaTime);
    }
}