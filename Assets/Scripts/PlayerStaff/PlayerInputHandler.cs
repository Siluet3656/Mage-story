using GameControl;
using UnityEngine;
using View;
using View.Buttons;

namespace PlayerStaff
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerTargeting))]
    [RequireComponent(typeof(PlayerSpellCasting))]
    public class PlayerInputHandler : PausableBehavior
    {
        [SerializeField] private SpellBarButton[] _spellBarButtons;

        private Camera _mainCamera;

        private PlayerInputActions _inputActions;

        private PlayerMovement _movement;
        private PlayerTargeting _targeting;
        private PlayerSpellCasting _spellCasting;
        private Menu _menu;

        private SpellDrag _hand;

        protected override void Awake()
        {
            base.Awake();
            
            _spellCasting = GetComponent<PlayerSpellCasting>();
            _movement = GetComponent<PlayerMovement>();
            _targeting = GetComponent<PlayerTargeting>();
            
            _inputActions = new PlayerInputActions();
            
            _inputActions.Player.Movement.performed += ctx =>  OnMovementPerformed(ctx.ReadValue<Vector2>());
            _inputActions.Player.Movement.canceled += ctx => OnMovementEnded();

            _inputActions.Player.Blink.started += ctx => OnBlinkButton();

            _inputActions.Player.FastTarget.started += ctx => OnFastTargetButton();
            
            _inputActions.UI.LBM.started += ctx => OnLeftMouseButtonClicked();
            
            _inputActions.Player.Castbar1.started += ctx => OnSpellBarButton(0);
            _inputActions.Player.Castbar2.started += ctx => OnSpellBarButton(1);
            _inputActions.Player.Castbar3.started += ctx => OnSpellBarButton(2);
            _inputActions.Player.Castbar4.started += ctx => OnSpellBarButton(3);
            _inputActions.Player.Castbar5.started += ctx => OnSpellBarButton(4);
            _inputActions.Player.Castbar6.started += ctx => OnSpellBarButton(5);
            _inputActions.Player.Castbar7.started += ctx => OnSpellBarButton(6);

            _inputActions.Player.CastInterrupt.started += ctx => OnCastInterrupt();
            
            _inputActions.UI.OpenSpellBook.performed += ctx => OnSpellBookOpen();
            
            _mainCamera = Camera.main;

            if (_mainCamera == null)
            {
                Debug.LogError("NO CAMERA!!!");
            }
            
            _hand = FindObjectsOfType<SpellDrag>()[0];
            _menu = FindObjectsOfType<Menu>()[0];
        }
        
        private void OnEnable() => _inputActions.Enable();
        private void OnDisable() => _inputActions.Disable();

        private void Update()
        {
            Vector2 point = _inputActions.UI.MousePosition.ReadValue<Vector2>();
            
            _hand.SetPoint(_mainCamera.ScreenToWorldPoint(point));
            _spellCasting.SetMousePosition(new Vector3(_mainCamera.ScreenToWorldPoint(point).x, _mainCamera.ScreenToWorldPoint(point).y, -3));
        }

        private void OnMovementPerformed(Vector2 value)
        {
            _movement.SetMovementInput(value);
        }

        private void OnMovementEnded()
        {
            _movement.SetMovementInput(Vector2.zero);
        }

        private void OnBlinkButton()
        {
            _movement.OnBlink();
        }

        private void OnFastTargetButton()
        {
            _targeting.OnFastTarget();
        }

        private void OnLeftMouseButtonClicked()
        {
            if (_hand.CheckDraggingStatus())
            {
                _hand.TryToDropASpell();
            }
            else if (_spellCasting.IsPlacing)
            {
                _spellCasting.Place();
            }
            else
            {
                Ray ray = _mainCamera.ScreenPointToRay(_inputActions.UI.MousePosition.ReadValue<Vector2>());
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("Targetable"));
                _targeting.OnMouseTargetSelect(hit);
            }
        }
        
        private void OnSpellBarButton(int buttonIndex)
        {
            if (buttonIndex >= 0 && buttonIndex < _spellBarButtons.Length)
            {
                _spellCasting.StartCast(_spellBarButtons[buttonIndex].GetSpellType());
            }
        }

        private void OnCastInterrupt()
        {
            if (_spellCasting.IsPlacing)
            {
                _spellCasting.CancelPlacing();
            }
            else
            {
                _spellCasting.StopCast();
            }
        }

        private void OnSpellBookOpen()
        {
            _menu.SwitchSpellBookState();
        }
    }
}