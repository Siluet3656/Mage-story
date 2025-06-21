using UnityEngine;

namespace PlayerStaff
{
    public class PlayerBuffHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _iceTombEffect;
    
        private PlayerMovement _movement;
        //private BuffSystem _buffSystem;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            /*_buffSystem = GetComponent<BuffSystem>();
        
            _buffSystem.OnPlayerFreeze += OnFreeze;
            _buffSystem.OnPlayerUnFreeze += OnUnfreeze;*/
        }

        private void OnFreeze()
        {
            _movement.DisableMovement();
            _iceTombEffect.SetActive(true);
        }

        private void OnUnfreeze()
        {
            _movement.EnableMovement();
            _iceTombEffect.SetActive(false);
        }
    }
}