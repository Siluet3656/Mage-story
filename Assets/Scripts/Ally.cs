using UnityEngine;
using EntityResources;
using UnityEngine.UI;
using View;

[RequireComponent(typeof(SpriteRenderer))]
public class Ally : MonoBehaviour
{
    [SerializeField] private GameObject _hpBarBg;
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

    public void Initialize(bool isTargetable, bool isNeedHp, int summonHp)
    {
        _targetMark.SetActive(isTargetable);
        _hpBarBg.SetActive(isNeedHp);

        if (isNeedHp)
        {
            Hp hp = gameObject.AddComponent<Hp>();
            hp.SetMaxHealth(summonHp);
            hp.InitializeHealth();

            HpView hpView = gameObject.GetComponent<HpView>();
            hpView.SetHealthBarImage(_hpBar.GetComponent<Image>());
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
