using System.Collections;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [Header("Panel")] 
    [SerializeField] private StatusPanel statusPanel;
    
    [Header("Buffs")]
    [SerializeField] private int fireAuraduration;
    
    private bool isFireAuraApplied = false;

    public void GetBuff(BuffType buff)
    {
        switch (buff)
        {
            case BuffType.FireAura:
                if (!isFireAuraApplied)
                {
                    //Crit ampify +
                    isFireAuraApplied = true;
                    statusPanel.AddStatus(BuffType.FireAura);
                    StartCoroutine(RemoveBuff(BuffType.FireAura));
                }
                break;
        }
    }
    
    private IEnumerator RemoveBuff(BuffType type)
    {
        switch (type)
        {
            case BuffType.FireAura:
                yield return new WaitForSeconds(fireAuraduration);
                //Crit ampify -
                isFireAuraApplied = false;
                statusPanel.RemoveStatus(BuffType.FireAura);
                break;
        }
    }
}
