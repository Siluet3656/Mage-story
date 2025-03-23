using System.Collections;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [Header("Panel")] 
    [SerializeField] private StatusPanel statusPanel;
    
    [Header("Buffs")]
    [SerializeField] private int fireAuraDuration;
    [SerializeField] private float fireAuraMultAdjust;
    [SerializeField] private float fireAuraChanceAdjust;
    
    private bool isFireAuraApplied = false;

    public void GetBuff(BuffType buff, Player player)
    {
        switch (buff)
        {
            case BuffType.FireAura:
                if (!isFireAuraApplied)
                {
                    player.SetCritAdjustFire(fireAuraMultAdjust,fireAuraChanceAdjust);
                    isFireAuraApplied = true;
                    statusPanel.AddStatus(BuffType.FireAura);
                    StartCoroutine(RemoveBuff(BuffType.FireAura, player));
                }
                break;
        }
    }
    
    private IEnumerator RemoveBuff(BuffType type, Player player)
    {
        switch (type)
        {
            case BuffType.FireAura:
                yield return new WaitForSeconds(fireAuraDuration);
                player.SetCritAdjustFire(1,1);
                isFireAuraApplied = false;
                statusPanel.RemoveStatus(BuffType.FireAura);
                break;
        }
    }
}
