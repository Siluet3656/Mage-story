using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SpellDrag : MonoBehaviour
{
    [SerializeField] private SpellTypeData data;
    [SerializeField] private GameObject corner;
    private PlayerInputActions playerInputActions;
    private SpellType draggingSpell;
    private Vector2 offset = new Vector2(0.5f,-0.5f);
    private Camera cam;
    private bool isDragging = false;
    private float waitabit;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.UI.LBM.performed += TryToDropASpell;
    }

    private void OnEnable()
    {
        playerInputActions.UI.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.UI.Disable();
    }

    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        if (isDragging)
        {
            Vector3 point = cam.ScreenToWorldPoint(playerInputActions.UI.MousePosition.ReadValue<Vector2>());
            point.x += offset.x;
            point.y += offset.y;
            point.z += 1f;
            transform.position = point;
        }
        waitabit = Time.fixedDeltaTime * 15;
    }
    public void TakeSpell(SpellType type)
    {
        draggingSpell = type;
        GetComponent<Image>().sprite = data.GetDataByType(type).Icon;
        this.GetComponent<Image>().color = new Color(1,1,1,1);
        corner.SetActive(true);
        isDragging = true;
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
        corner.SetActive(false);
        isDragging = false;
        Cursor.visible = true;
    }

    private void TryToDropASpell(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
            StartCoroutine(WaitaBit());
        }
    }
    
    private IEnumerator WaitaBit()
    {
        yield return new WaitForSeconds(waitabit);
        DropSpell();
    }

    public bool GetIsDragging()
    {
        return this.isDragging;
    }

    public SpellType GetSpellType()
    {
        return this.draggingSpell;
    }
}
