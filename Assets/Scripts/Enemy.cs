using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isTargeted = false;
    [SerializeField]private SpriteRenderer sr = null;
    private Color OriginalColor;
    private Color TargetedColor;

    private void Start()
    {
        OriginalColor = sr.color;
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
        sr.color = TargetedColor;
    }

    public void ResetTarget()
    {
        isTargeted = false;
        sr.color = OriginalColor;
    }

    public bool CheckTargetStatus()
    {
        return this.isTargeted;
    }
}
