using UnityEngine;

namespace GameControl
{
    public abstract class PausableBehavior : MonoBehaviour
    {
        [SerializeField] private bool _pausable;

        private bool _initialized = false;
        
        private void OnPause()
        {
            if (_pausable == false) return;
            enabled = false;
            PostPause();
        }
        
        private void OnResume()
        {
            if (_pausable == false) return;
            enabled = true;
            PostResume();
        }
        
        protected virtual void Awake()
        {
            if (_pausable == false) return;
            if (MenuInputs.Instance == null) return;

            MenuInputs.Instance.OnPause += OnPause;
            MenuInputs.Instance.OnResume += OnResume;

            _initialized = true;

            enabled = !MenuInputs.Instance.IsPaused;
        }

        protected virtual void OnDestroy()
        {
            if (_pausable == false) return;
            if (_initialized == false) return;
            
            MenuInputs.Instance.OnPause -= OnPause;
            MenuInputs.Instance.OnResume -= OnResume;
        }
        
        protected virtual void PostPause() {}
        protected virtual void PostResume() {}
        protected virtual void CleanUp() {}
    }
}
