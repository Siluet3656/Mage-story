using System;
using Data;
using UnityEngine;
using UnityEngine.UI;
using Data.Enums;
using PlayerStaff;

namespace View.Buttons
{
    [RequireComponent(typeof(Image))]
    public class SpellBarButton : MonoBehaviour
    {
        private Image _iconPlace;
        private SpellDrag _hand;
        private SpellName _currentSpell;
        
        private PlayerSpellCasting _playerSpellCasting;

        private void Awake()
        {
            _hand = FindObjectsOfType<SpellDrag>()[0];
            _iconPlace = GetComponent<Image>();
        }

        private void Start()
        {
            _playerSpellCasting = G.Player.GetComponent<PlayerSpellCasting>();
        }

        public void PlaceSpell()
        {
            if (_hand.GetIsDragging())
            {
                _hand.PlaceSpell(_iconPlace);
                _currentSpell = _hand.GetSpellType();
            }
        }

        public SpellName GetSpellType()
        {
            return _currentSpell;
        }

        public void ManualCast()
        {
            _playerSpellCasting.StartCast(_currentSpell);
        }
    }
}
