using System;
using UnityEngine;

namespace EnemyStaff
{
    public interface ITargetable
    {
        bool IsTargetable { get; }
        bool IsTargeted { get; }
    
        void OnTargeted();
        void OnUntargeted();
        
        GameObject GameObject { get; }

        event Action OnTargetDestroy;
    }
}