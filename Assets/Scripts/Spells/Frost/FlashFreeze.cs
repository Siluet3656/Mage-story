using UnityEngine;
using Data.Enums;
using EnemyStaff;
using Statuses;

namespace Spells.Frost
{
    [RequireComponent(typeof(StatusApplier))]
    public class FlashFreeze : AoeInstantSpell
    {
        private StatusApplier _statusApplier;
        protected override void Awake()
        {
            base.Awake();

            _statusApplier = GetComponent<StatusApplier>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<EnemyTargeting>(out _))
            {
                if (other.TryGetComponent(typeof(StatusController),out _))
                {
                    _statusApplier.ApplyStatusToTarget(StatusType.Slow, other.gameObject);
                }
            }
        }
    }
}
