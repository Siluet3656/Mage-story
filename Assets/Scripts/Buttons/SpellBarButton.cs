using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellBarButton : MonoBehaviour, IPointerClickHandler
{
    private SpellDrag hand;
    private SpellType currentSpell;

    private void Start()
    {
        hand = FindObjectsOfType<SpellDrag>()[0];
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (hand.GetIsDragging())
        {
            hand.PlaceSpell(this.GetComponent<Image>());
            this.currentSpell = hand.GetSpellType();
        }
    }

    public SpellType GetSpellType()
    {
        return this.currentSpell;
    }
}
