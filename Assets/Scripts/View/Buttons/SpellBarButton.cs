using System;
using System.Collections;
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

        private bool _isManualCastBlocked;

        private void Awake()
        {
            _hand = FindObjectsOfType<SpellDrag>()[0];
            _iconPlace = GetComponent<Image>();

            _isManualCastBlocked = true;
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
                StartCoroutine(Wait());
            }
        }

        private IEnumerator Wait()
        {
            _isManualCastBlocked = true;
            yield return new WaitForSeconds(0.2f);
            _isManualCastBlocked = false;
        }

        public SpellName GetSpellType()
        {
            return _currentSpell;
        }

        public void ManualCast()
        {
            if (_isManualCastBlocked) return;
            
            _playerSpellCasting.StartCast(_currentSpell);
        }
    }
}
