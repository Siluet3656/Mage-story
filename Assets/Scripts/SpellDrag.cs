using System;
using UnityEngine;
using UnityEngine.UI;

public class SpellDrag : MonoBehaviour
{
    [SerializeField] private SpellTypeData data;
    private float offset;
    private float hzcho = 100;
    private float screenX = Screen.width;
    private float screenY = Screen.height;
    
    public void TakeSpell(SpellType type)
    {
        GetComponent<Image>().sprite = data.GetDataByID(type);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
