using UnityEngine;

public class CombatStartButton : MonoBehaviour
{
    [Header("References")]
    public CombatStarter combatStarter;
    
    private void OnMouseEnter()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = new Color(1f, 1f, 0.5f, 1f);
            transform.localScale = Vector3.one * 1.1f;
        }
    }
    
    private void OnMouseExit()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = Color.white;
            transform.localScale = Vector3.one;
        }
    }
    
    private void OnMouseDown()
    {
        if (combatStarter == null)
            combatStarter = FindObjectOfType<CombatStarter>();
        
        if (combatStarter != null)
        {
            combatStarter.StartCombatSequence();
        }
    }
}
