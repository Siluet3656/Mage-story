using UnityEngine;

namespace Animations
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimations : MonoBehaviour
    {
        private Animator _animator;
        private Vector2 _movement;

        public void SetMovementVector(Vector2 value)
        {
            _movement = value;
        }
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
            _movement = Vector2.zero;
        }

        private void Update()
        {
            _animator.SetFloat("MoveX", _movement.x);
            _animator.SetFloat("MoveY",  _movement.y);
        }
    }
}
