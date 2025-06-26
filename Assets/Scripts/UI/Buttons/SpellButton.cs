using Data.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UI.Buttons
{
    public class SpellButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [FormerlySerializedAs("_type")] [SerializeField] private SpellName _name;
        private SpellDrag _hand;

        private void Start()
        {
            _hand = FindObjectsOfType<SpellDrag>()[0];
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_name != SpellName.NoSpell)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    _hand.TakeSpell(_name);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        
        }

        public void OnDrag(PointerEventData eventData)
        {
        
        }
    }
}
