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
            
            var menuInputs = MenuInputs.Instance;
            if (menuInputs == null) return;

            menuInputs.OnPause += OnPause;
            menuInputs.OnResume += OnResume;

            _initialized = true;

            enabled = !menuInputs.IsPaused;
        }

        protected virtual void OnDestroy()
        {
            if (_pausable == false) return;
            if (_initialized == false) return;
            
            var menuInputs = MenuInputs._instance;
            if (menuInputs == null) return;
            
            menuInputs.OnPause -= OnPause;
            menuInputs.OnResume -= OnResume;
        }
        
        protected virtual void PostPause() {}
        protected virtual void PostResume() {}
        protected virtual void CleanUp() {}
    }
}
