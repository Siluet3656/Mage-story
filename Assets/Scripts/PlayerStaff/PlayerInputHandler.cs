using UnityEngine;

namespace PlayerStaff
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private SpellBarButton[] _spellBarButtons;
    
        private PlayerSpellcasting _spellCasting;
        private PlayerMovement _movement;

        private void Awake()
        {
            _spellCasting = GetComponent<PlayerSpellcasting>();
            _movement = GetComponent<PlayerMovement>();
        
            PlayerInputActions inputActions = new PlayerInputActions();
        
            inputActions.Player.Castbar1.started += ctx => OnSpellBarButton(0);
            inputActions.Player.Castbar2.started += ctx => OnSpellBarButton(1);
            inputActions.Player.Castbar3.started += ctx => OnSpellBarButton(2);
            inputActions.Player.Castbar4.started += ctx => OnSpellBarButton(3);
            inputActions.Player.Castbar5.started += ctx => OnSpellBarButton(4);
            inputActions.Player.Castbar6.started += ctx => OnSpellBarButton(5);
            inputActions.Player.Castbar7.started += ctx => OnSpellBarButton(6);
        
            inputActions.Enable();
        }

        private void OnSpellBarButton(int buttonIndex)
        {
            if (buttonIndex >= 0 && buttonIndex < _spellBarButtons.Length)
            {
                _spellCasting.CastSpell(_spellBarButtons[buttonIndex].GetSpellType());
            }
        }
    }
}