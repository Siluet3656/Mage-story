using UnityEngine;
using EntityStaff;
using Data;
using Data.Enums;

namespace EnemyStaff
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private EnemyProjectileName _enemyProjectileName;
        
        private const float Speed = 800f;
        private const float MinimumDistance = 0.3f;
        
        private Transform _playerTransform;
        private Rigidbody2D _rigidbody;
        private Hp _playerHp;
        
        private Vector3 _direction;
        private float _angle;

        private float _damage;

        protected void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerTransform = G.Player.transform;
            _playerHp = G.PlayersHp;
        }
        
        private void FixedUpdate()
        {
            if (_playerTransform == null) return;

            _direction = _playerTransform.position - transform.position;
            Move();
            
            if (Vector2.Distance(transform.position, _playerTransform.position) < MinimumDistance)
            {
                OnReachTarget();
            }
        }
        
        private void Move()
        {
            _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            _rigidbody.velocity = _direction.normalized * (Speed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
        }
        
        private void OnReachTarget()
        {
            _playerHp.TryToTakeDamage(_damage, false);
            EnemyProjectilesFactory.Instance.ReturnProjectile(_enemyProjectileName,this);
        }

        public void SetDamage(float damage) => _damage = damage;
    }
}
