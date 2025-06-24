using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Data.Enums;
using UnityEngine.UI;

namespace UI
{
    public class SpellDrag : MonoBehaviour
    {
        [SerializeField] private GameObject _corner;
        private PlayerInputActions _playerInputActions;
        private SpellType _draggingSpell;
        private readonly Vector2 _offset = new Vector2(0.5f,-0.5f);
        private Camera _cam;
        private bool _isDragging = false;
        private float _waitabit;

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.UI.Lbm.performed += TryToDropASpell;
        }

        private void OnEnable()
        {
            _playerInputActions.UI.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions.UI.Disable();
        }

        private void Start()
        {
            _cam = Camera.main;
        }
        private void Update()
        {
            if (_isDragging)
            {
                Vector3 point = _cam.ScreenToWorldPoint(_playerInputActions.UI.MousePosition.ReadValue<Vector2>());
                point.x += _offset.x;
                point.y += _offset.y;
                point.z += 1f;
                transform.position = point;
            }
            _waitabit = Time.fixedDeltaTime * 15;
        }
        public void TakeSpell(SpellType type)
        {
            _draggingSpell = type;
            GetComponent<Image>().sprite = _data.GetDataByType(type).Icon;
            this.GetComponent<Image>().color = new Color(1,1,1,1);
            _corner.SetActive(true);
            _isDragging = true;
            Cursor.visible = false;
            StopCoroutine("WaitaBit");
        }
        public void PlaceSpell(Image toplace)
        {
            toplace.sprite = this.GetComponent<Image>().sprite;
            DropSpell();
        }
        private void DropSpell()
        {
            this.GetComponent<Image>().color = new Color(1,1,1,0);
            _corner.SetActive(false);
            _isDragging = false;
            Cursor.visible = true;
        }

        private void TryToDropASpell(InputAction.CallbackContext context)
        {
            if (_isDragging)
            {
                StartCoroutine(WaitaBit());
            }
        }
    
        private IEnumerator WaitaBit()
        {
            yield return new WaitForSeconds(_waitabit);
            DropSpell();
        }

        public bool GetIsDragging()
        {
            return this._isDragging;
        }

        public SpellType GetSpellType()
        {
            return this._draggingSpell;
        }
    }
}
