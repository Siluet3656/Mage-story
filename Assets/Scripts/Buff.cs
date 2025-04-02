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
    [Space] 
    [SerializeField] private int stasysFreezeDuration;
    
    private bool isFireAuraApplied = false;
    private bool isStasisFreezeApplied = false;

    public delegate void DoPlayerFreeze();
    public delegate void DoPlayerUnFreeze();
    public event DoPlayerFreeze OnPlayerFreeze;
    public event DoPlayerUnFreeze OnPlayerUnFreeze;

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
            case BuffType.StasisFreeze:
                if (!isStasisFreezeApplied)
                {
                    OnPlayerFreeze?.Invoke();
                    isStasisFreezeApplied = true;
                    statusPanel.AddStatus(BuffType.StasisFreeze);
                    StartCoroutine(RemoveBuff(BuffType.StasisFreeze, player));
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
            case BuffType.StasisFreeze:
                yield return new WaitForSeconds(stasysFreezeDuration);
                OnPlayerUnFreeze?.Invoke();
                isStasisFreezeApplied = false;
                statusPanel.RemoveStatus(BuffType.StasisFreeze);
                break;
        }
    }
}
