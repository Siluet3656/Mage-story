using UnityEngine;

namespace GameControl
{
    public abstract class PausableBehavior : MonoBehaviour
    {
        [SerializeField] private bool _pausable;

        private bool _initialized = false;

        protected void Awake()
        {
            if (_pausable == false) return;
            //if ()

            _initialized = true;
            
            //enabled
        }

        private void OnDestroy()
        {
            if (_pausable == false) return;
            if (_initialized == false) return;
        }

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
        
        protected virtual void PostPause() {}
        protected virtual void PostResume() {}
        protected virtual void CleanUp() {}
    }
}
