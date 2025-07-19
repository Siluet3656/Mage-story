using UnityEngine;
using EntityResources;

[RequireComponent(typeof(SpriteRenderer))]
public class Ally : MonoBehaviour
{
    [SerializeField] private GameObject _hpBar;
    [SerializeField] private GameObject _targetMark;
    [SerializeField] private Color _targetedColor;

    private SpriteRenderer _spriteRenderer;
    private Color _defaultColor;

    private void Awake()
    {
        _spriteRenderer = _targetMark.GetComponent<SpriteRenderer>();
        _defaultColor = _spriteRenderer.color;
    }

    public void Initialize(bool isTargetable, bool isNeedHp, float summonHp)
    {
        _targetMark.SetActive(isTargetable);
        _hpBar.SetActive(isNeedHp);

        if (isNeedHp)
        {
            Hp hp = gameObject.AddComponent<Hp>();
            hp.InitializeHealth(summonHp);
        }
    }

    public void OnTargeted()
    {
        _spriteRenderer.color = _targetedColor;
    }
    
    public void OnUntargeted()
    {
        _spriteRenderer.color = _defaultColor;
    }
}
