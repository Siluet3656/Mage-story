using UnityEngine;
using UI;
using UI.Buttons;

namespace PlayerStaff
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerTargeting))]
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private SpellBarButton[] _spellBarButtons;

        private Camera _mainCamera;

        private PlayerInputActions _inputActions;

        private PlayerMovement _movement;
        private PlayerTargeting _targeting;
        private PlayerSpellCasting _spellCasting;

        private SpellDrag _hand;

        private void Awake()
        {
            _spellCasting = GetComponent<PlayerSpellCasting>();
            _movement = GetComponent<PlayerMovement>();
            _targeting = GetComponent<PlayerTargeting>();
            
            _inputActions = new PlayerInputActions();
            
            _inputActions.Player.Movement.performed += ctx =>  OnMovementPerformed(ctx.ReadValue<Vector2>());
            _inputActions.Player.Movement.canceled += ctx => OnMovementEnded();

            _inputActions.Player.Blink.started += ctx => OnBlinkButton();

            _inputActions.Player.FastTarget.started += ctx => OnFastTargetButton();
            _inputActions.UI.Lbm.started += ctx => OnLeftMouseButtonClicked();
            
            _inputActions.Player.Castbar1.started += ctx => OnSpellBarButton(0);
            _inputActions.Player.Castbar2.started += ctx => OnSpellBarButton(1);
            _inputActions.Player.Castbar3.started += ctx => OnSpellBarButton(2);
            _inputActions.Player.Castbar4.started += ctx => OnSpellBarButton(3);
            _inputActions.Player.Castbar5.started += ctx => OnSpellBarButton(4);
            _inputActions.Player.Castbar6.started += ctx => OnSpellBarButton(5);
            _inputActions.Player.Castbar7.started += ctx => OnSpellBarButton(6);

            _inputActions.Player.CastInterrupt.started += ctx => OnCastInterrupt();

            _inputActions.UI.MousePosition.performed += ctx => OnMouseMove(ctx.ReadValue<Vector2>());
            
            _mainCamera = Camera.main;

            if (_mainCamera == null)
            {
                Debug.Log("NO CAMERA!!!");
            }
            
            _hand = FindObjectsOfType<SpellDrag>()[0];
        }
        
        private void OnEnable() => _inputActions.Enable();
        private void OnDisable() => _inputActions.Disable();

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
            else
            {
                Ray ray = _mainCamera.ScreenPointToRay(_inputActions.UI.MousePosition.ReadValue<Vector2>());
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                _targeting.OnMouseTargetSelect(hit);
            }
        }

        private void OnMouseMove(Vector2 point)
        {
            _hand.SetPoint(_mainCamera.ScreenToWorldPoint(point));
        }
        
        private void OnSpellBarButton(int buttonIndex)
        {
            if (buttonIndex >= 0 && buttonIndex < _spellBarButtons.Length)
            {
                _spellCasting.CastStart(_spellBarButtons[buttonIndex].GetSpellType());
            }
        }

        private void OnCastInterrupt()
        {
            _spellCasting.StopCast();
        }
    }
}