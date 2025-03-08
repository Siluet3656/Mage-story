using System;
using UnityEngine;
using UnityEngine.UI;

public class SpellDrag : MonoBehaviour
{
    [SerializeField] private SpellTypeData data;
    private Vector2 offset = new Vector2(0.5f,-0.5f);
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    public void TakeSpell(SpellType type)
    {
        GetComponent<Image>().sprite = data.GetDataByID(type);
    }

    private void Update()
    {
        Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);
        point.x += offset.x;
        point.y += offset.y;
        point.z += 1f;
        transform.position = point;
    }
}
