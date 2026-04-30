using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameControl
{
    public class MenuInputs : MonoBehaviour
    {
        private static MenuInputs _instance;
        
        public static MenuInputs Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MenuInputs>();
                }
                return _instance;
            }
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
        
        public void Pause()
        {
            _isPaused = true;
            OnPause?.Invoke();
            _menu.OpenSceneMenu();
            
            Time.timeScale = 0f;
        }
        
        public void Resume()
        {
            _isPaused = false;
            OnResume?.Invoke();
            _menu.CloseSceneMenu();
            
            Time.timeScale = 1f;
        }
        
        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Resume();
        }
    }
}
