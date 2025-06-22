using Data.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellBarButton : MonoBehaviour, IPointerClickHandler
{
    private SpellDrag _hand;
    private SpellType _currentSpell;

    private void Start()
    {
        _hand = FindObjectsOfType<SpellDrag>()[0];
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_hand.GetIsDragging())
        {
            _hand.PlaceSpell(this.GetComponent<Image>());
            this._currentSpell = _hand.GetSpellType();
        }
    }

    public SpellType GetSpellType()
    {
        return this._currentSpell;
    }
}
