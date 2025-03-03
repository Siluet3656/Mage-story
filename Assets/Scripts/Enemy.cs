using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]private Vector2[] PatrolPoints;
    [SerializeField]private SpriteRenderer TargetedSR = null;
    private Color OriginalColor;
    private Color TargetedColor;
    private bool isTargeted = false;

    private void Start()
    {
        OriginalColor = TargetedSR.color;
        TargetedColor = new Color(1, 0, 0, OriginalColor[3]);
    }

    private void OnDestroy() 
    {
        var SpellsToClear = FindObjectsOfType<Spell>();
        Enemy nullenemy = null;
        for (var i = 0; i < SpellsToClear.Length; i++)
        {
            if(SpellsToClear[i].GetTarget().name == this.name)
            {
                SpellsToClear[i].SetTarget(nullenemy);
            }
        }
    }

    public void Target()
    {
        isTargeted = true;
        TargetedSR.color = TargetedColor;
    }

    public void ResetTarget()
    {
        isTargeted = false;
        TargetedSR.color = OriginalColor;
    }

    public bool CheckTargetStatus()
    {
        return this.isTargeted;
    }
}
