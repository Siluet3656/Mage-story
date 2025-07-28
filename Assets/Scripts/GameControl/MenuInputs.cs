using System;
using UnityEngine;

namespace GameControl
{
    public class MenuInputs : MonoBehaviour
    {
        public static MenuInputs Instance;
        MenuInputs()
        {
            if (Instance != null) return;

            Instance = this;
        }
        
        private PlayerInputActions _inputActions;
        private Menu _menu;
        private bool _isPaused;
        
        private void Awake()
        {
            _inputActions = new PlayerInputActions();
            _menu = FindObjectsOfType<Menu>()[0];
            
            _inputActions.UI.Closebutton.performed += ctx => OnCloseButton();

            _isPaused = false;
        }
        
        private void OnEnable() => _inputActions.Enable();
        private void OnDisable() => _inputActions.Disable();
        
        private void OnCloseButton()
        {
            if (_menu.IsBookOpened)
            {
                _menu.CloseSpellBook();
            }
            else
            {
                if (_isPaused) Resume(); else Pause();
            }
        }

        public bool IsPaused => _isPaused;
        public Action OnPause;
        public Action OnResume;
        
        private void Pause()
        {
            _isPaused = true;
            OnPause?.Invoke();

            Time.timeScale = 0f;
        }
        
        private void Resume()
        {
            _isPaused = false;
            OnResume?.Invoke();
            
            Time.timeScale = 1f;
        }
    }
}
