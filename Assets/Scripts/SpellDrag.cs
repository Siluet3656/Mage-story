using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellDrag : MonoBehaviour
{
    [SerializeField] private SpellTypeData data;
    [SerializeField] private GameObject corner;
    private SpellType draggingSpell;
    private Vector2 offset = new Vector2(0.5f,-0.5f);
    private Camera cam;
    private bool isDragging = false;
    private float waitabit;

    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        if (isDragging)
        {
            Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);
            point.x += offset.x;
            point.y += offset.y;
            point.z += 1f;
            transform.position = point;
        }

        if (isDragging)
        {
            waitabit = Time.fixedDeltaTime * 15;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine("WaitaBit");
            }
        }
    }
    public void TakeSpell(SpellType type)
    {
        draggingSpell = type;
        GetComponent<Image>().sprite = data.GetDataByID(type);
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
