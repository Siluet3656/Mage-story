using UnityEngine;

namespace EnemyStaff
{
    public interface ITargetble
    {
        bool IsTargetable { get; }
        bool IsTargeted { get; }
    
        void OnTargeted();
        void OnUntargeted();
        
        GameObject GameObject { get; }
    }
}