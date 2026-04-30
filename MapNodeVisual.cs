using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MapNodeVisual : MonoBehaviour
{
    public NodeType nodeType;
    
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (circleCollider == null)
            circleCollider = gameObject.AddComponent<CircleCollider2D>();
        
        circleCollider.isTrigger = true;
        circleCollider.radius = 0.3f;
    }
    
    private void OnMouseEnter()
    {
        if (spriteRenderer != null)
        {
            transform.localScale = Vector3.one * 1.15f;
        }
    }
    
    private void OnMouseExit()
    {
        if (spriteRenderer != null)
        {
            transform.localScale = Vector3.one;
        }
    }
    
    private void OnMouseDown()
    {
        if (CombatStarter.Instance != null)
        {
            // Get the MapNode data from the CombatStarter
            MapNode node = CombatStarter.Instance.currentMapData?.nodes?.Find(n => 
                n.gameObject == gameObject
            );
            
            if (node != null)
            {
                CombatStarter.Instance.SelectNode(node);
            }
        }
    }
}
