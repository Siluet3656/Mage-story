using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isTargeted = false;
    [SerializeField]private SpriteRenderer sr = null;
    private Color OriginalColor;
    private Color TargetedColor;

    void Start()
    {
        OriginalColor = sr.color;
        TargetedColor = new Color(1, 0, 0, OriginalColor[3]);
    }

    void Update()
    {

    }

    public void Target ()
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
