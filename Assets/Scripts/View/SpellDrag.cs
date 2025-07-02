using System.Collections;
using UnityEngine;
using Data;
using Data.Enums;
using Data.SpellConfigs;
using UnityEngine.UI;

namespace View
{
    public class SpellDrag : MonoBehaviour
    {
        private GameObject _corner;
        private Image _handIcon;
        private SpellName _draggingSpell;
        private Vector3 _point;
        private bool _isDragging;
        // Костыли
        private readonly Vector2 _offset = new Vector2(0.5f,-0.5f);

        private void Awake()
        {
            _corner = transform.GetChild(0).gameObject;
            _handIcon = GetComponent<Image>();
            
            _isDragging = false;
        }

        private void Update()
        {
            if (_isDragging)
            {
                transform.position = _point;
            }
        }
       
        private void DropSpell()
        {
            Cursor.visible = true;
            _handIcon.color = new Color(1,1,1,0);
            _corner.SetActive(false);
            _isDragging = false;
        }
        
        private IEnumerator WaitaBit()
        {
            yield return new WaitForSeconds(Time.deltaTime * 30);
            DropSpell();
        }
        
        public void TakeSpell(SpellName spellName)
        {
            Cursor.visible = false;
            _handIcon.color = new Color(1,1,1,1);
            _corner.SetActive(true);
            _isDragging = true;
            
            SpellConfig config = SpellData.Instance.GetSpellConfig(spellName);

            if (config == null)
            {
                DropSpell();
                return;
            }
            
            _draggingSpell = spellName;
            _handIcon.sprite = config.Icon;
        }
        
        public void TryToDropASpell()
        {
            if (_isDragging)
            {
                StartCoroutine(WaitaBit());
            }
        }
        
        public void PlaceSpell(Image place)
        {
            place.sprite = GetComponent<Image>().sprite;
        }
        
        public bool GetIsDragging()
        {
            return _isDragging;
        }

        public SpellName GetSpellType()
        {
            return _draggingSpell;
        }

        public bool CheckDraggingStatus()
        {
            return _isDragging;
        }

        public void SetPoint(Vector2 point)
        {
            _point = point;
            _point.x += _offset.x;
            _point.y += _offset.y;
        }
    }
}
