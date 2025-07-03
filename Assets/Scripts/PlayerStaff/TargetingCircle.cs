using System.Collections.Generic;
using EnemyStaff;
using UnityEngine;

namespace PlayerStaff
{
    public class TargetingCircle : MonoBehaviour
    {
        private readonly List<ITargetble> _nearbyTargets = new List<ITargetble>();

        public List<ITargetble> NearbyTargets => _nearbyTargets;
        private void OnTriggerEnter2D(Collider2D other)
        {
            ITargetble target = other.GetComponent<ITargetble>();
            if (target != null && _nearbyTargets.Contains(target) == false)
            {
                _nearbyTargets.Add(target);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            ITargetble target = other.GetComponent<ITargetble>();
            if (target != null && _nearbyTargets.Contains(target))
            {
                _nearbyTargets.Remove(target);
            }
        }
    }
}
