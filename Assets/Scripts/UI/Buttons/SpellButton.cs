using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private SpellType type;
    private SpellDrag _hand;

    private void Start()
    {
        _hand = FindObjectsOfType<SpellDrag>()[0];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _hand.TakeSpell(type);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
