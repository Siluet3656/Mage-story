using UnityEngine;

namespace Spells.Earth
{
    public class DeathZone : DeployableSpell
    {
        private readonly float _size = 5f;
        private Vector3 _defaultScale;

        protected override void Awake()
        {
            _defaultScale = transform.localScale;
            
            CircleCollider2D collider2d = gameObject.AddComponent<CircleCollider2D>();
            collider2d.isTrigger = true;
            collider2d.radius = 0.45f;
            
            base.Awake();
        }
    }
}
