using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private SpellType type;
    private SpellDrag hand;

    private void Start()
    {
        hand = FindObjectsOfType<SpellDrag>()[0];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            hand.TakeSpell(this.type);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
