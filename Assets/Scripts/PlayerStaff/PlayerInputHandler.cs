using System;
using UnityEngine;

namespace PlayerStaff
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerInputHandler : MonoBehaviour
    {
        public Action BlinkButtonPressed;
        
        [SerializeField] private SpellBarButton[] _spellBarButtons;

        private PlayerInputActions _inputActions;
        
        private PlayerSpellcasting _spellCasting;
        private PlayerMovement _movement;

        private void Awake()
        {
            _spellCasting = GetComponent<PlayerSpellcasting>();
            _movement = GetComponent<PlayerMovement>();
            _inputActions = new PlayerInputActions();
            
            _inputActions.Player.Movement.performed += ctx =>  OnMovementPerformed(ctx.ReadValue<Vector2>());
            _inputActions.Player.Movement.canceled += ctx => OnMovementEnded();

            _inputActions.Player.Blink.started += ctx => BlinkButtonPressed?.Invoke();
            
            _inputActions.Player.Castbar1.started += ctx => OnSpellBarButton(0);
            _inputActions.Player.Castbar2.started += ctx => OnSpellBarButton(1);
            _inputActions.Player.Castbar3.started += ctx => OnSpellBarButton(2);
            _inputActions.Player.Castbar4.started += ctx => OnSpellBarButton(3);
            _inputActions.Player.Castbar5.started += ctx => OnSpellBarButton(4);
            _inputActions.Player.Castbar6.started += ctx => OnSpellBarButton(5);
            _inputActions.Player.Castbar7.started += ctx => OnSpellBarButton(6);
        }

        private void OnMovementPerformed(Vector2 value)
        {
            _movement.SetMovementInput(value);
        }

        private void OnMovementEnded()
        {
            _movement.SetMovementInput(Vector2.zero);
        }

        private void OnEnable() => _inputActions.Enable();
        private void OnDisable() => _inputActions.Disable();

        private void OnSpellBarButton(int buttonIndex)
        {
            if (buttonIndex >= 0 && buttonIndex < _spellBarButtons.Length)
            {
                _spellCasting.CastSpell(_spellBarButtons[buttonIndex].GetSpellType());
            }
        }
    }
}