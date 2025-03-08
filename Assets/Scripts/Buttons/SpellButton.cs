using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private SpellType type;
    private SpellDrag hand;

    private void Start()
    {
        hand = FindObjectsOfType<SpellDrag>()[0];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            hand.TakeSpell(this.type);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
