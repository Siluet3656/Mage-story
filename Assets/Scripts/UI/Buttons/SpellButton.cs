using Data.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class SpellButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [FormerlySerializedAs("type")] [SerializeField] private SpellType _type;
    private SpellDrag _hand;

    private void Start()
    {
        _hand = FindObjectsOfType<SpellDrag>()[0];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_type != SpellType.NoSpell)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _hand.TakeSpell(_type);
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
