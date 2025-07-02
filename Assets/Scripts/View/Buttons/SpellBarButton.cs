using Data.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View.Buttons
{
    [RequireComponent(typeof(Image))]
    public class SpellBarButton : MonoBehaviour, IPointerClickHandler
    {
        private Image _icon;
        private SpellDrag _hand;
        private SpellName _currentSpell;

        private void Awake()
        {
            _hand = FindObjectsOfType<SpellDrag>()[0];
            _icon = GetComponent<Image>();
        }
    
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_hand.GetIsDragging())
            {
                _hand.PlaceSpell(_icon);
                _currentSpell = _hand.GetSpellType();
            }
        }

        public SpellName GetSpellType()
        {
            return _currentSpell;
        }
    }
}
