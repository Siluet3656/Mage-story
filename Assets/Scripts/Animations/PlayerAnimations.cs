using UnityEngine;
using PlayerStaff;

namespace Animations
{
    public class PlayerAnimations : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private Animator _animator;
        private void Awake()
        {
            _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            _animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetFloat("MoveX", _playerMovement.GetMovementInput().x);
            _animator.SetFloat("MoveY",  _playerMovement.GetMovementInput().y);
        }
    }
}
