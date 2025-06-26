using UnityEngine;

namespace Spells
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ProjectileSpell : Spell
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _minimumDistance;
        [SerializeField] private Sprite _sprite;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Enemy _target;
        private Vector3 _direction;
        private float _angle;

        protected override void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (!_target) return;

            _direction = _target.transform.position - transform.position;
            Move();
            
            if (Vector2.Distance(transform.position, _target.transform.position) < _minimumDistance)
            {
                OnReachTarget();
            }
        }

        private void OnReachTarget()
        {
            ReturnToPool();
        }

        private void Move()
        {
            _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            _rigidbody.velocity = _direction.normalized * (_speed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
        }

        public bool SetTarget(Enemy target)
        {
            if (target == null) return false;
            
            _target = target;

            return true;
        }
        
        public override void DoSpell()
        {
            if (_target == null) return;
            
            _spriteRenderer.sprite = _sprite;
            gameObject.SetActive(true);
        }
    }
}