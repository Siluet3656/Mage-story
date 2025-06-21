using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image _blinkRefreshBar;

        public void SetBlinkRefreshBarFillAmount(float value)
        {
            if (value < 0f)
            {
                _blinkRefreshBar.fillAmount = 0f;
                return;
            }
            if (value > 1f)
            {
                _blinkRefreshBar.fillAmount = 1f;
                return;
            }
            _blinkRefreshBar.fillAmount = value;
        }
    }
}
