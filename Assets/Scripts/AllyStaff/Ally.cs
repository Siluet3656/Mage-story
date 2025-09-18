using EntityStaff;
using Statuses;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace AllyStaff
{
    [RequireComponent(typeof(ViewCatcher))]
    public class Ally : MonoBehaviour
    {
        [SerializeField] private GameObject _hpBarBg;
        [SerializeField] private GameObject _hpBar;
        [SerializeField] private GameObject _targetMark;
        [SerializeField] private Color _targetedColor;

        private ViewCatcher _viewCatcher;
        private Color _defaultColor;

        private void Awake()
        {
            _viewCatcher = GetComponent<ViewCatcher>();
            _defaultColor = _viewCatcher.SpriteRenderer.color;
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

                gameObject.AddComponent<StatusController>();
            }
        }

        public void OnTargeted()
        {
            _viewCatcher.SpriteRenderer.color = _targetedColor;
        }
    
        public void OnUntargeted()
        {
            _viewCatcher.SpriteRenderer.color = _defaultColor;
        }
    }
}
