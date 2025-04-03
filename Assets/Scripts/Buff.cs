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
    public delegate void DoEnemyFreeze();
    public delegate void DoEnemyUnFreeze();
    public event DoEnemyFreeze OnEnemyFreeze;
    public event DoEnemyUnFreeze OnEnemyUnFreeze;

    public void GetBuff(BuffType buff, Player player)
    {
        switch (buff)
        {
            case BuffType.FireAura:
                if (!isFireAuraApplied)
                {
                    player.SetCritAdjustFire(fireAuraMultAdjust,fireAuraChanceAdjust);
                    isFireAuraApplied = true;
                    statusPanel.AddStatus(BuffType.FireAura, player);
                    StartCoroutine(RemoveBuff(BuffType.FireAura, player));
                }
                break;
            case BuffType.StasisFreeze:
                if (!isStasisFreezeApplied)
                {
                    OnPlayerFreeze?.Invoke();
                    isStasisFreezeApplied = true;
                    statusPanel.AddStatus(BuffType.StasisFreeze, player);
                    StartCoroutine(RemoveBuff(BuffType.StasisFreeze, player));
                }
                break;
        }
    }
    
    public void GetBuff(BuffType buff, Enemy enemy)
    {
        switch (buff)
        {
            case BuffType.StasisFreeze:
                if (!isStasisFreezeApplied)
                {
                    OnEnemyFreeze?.Invoke();
                    isStasisFreezeApplied = true;
                    statusPanel.AddStatus(BuffType.StasisFreeze, enemy);
                    StartCoroutine(RemoveBuff(BuffType.StasisFreeze, enemy));
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
                statusPanel.RemoveStatus(BuffType.FireAura, player);
                break;
            case BuffType.StasisFreeze:
                yield return new WaitForSeconds(stasysFreezeDuration);
                OnPlayerUnFreeze?.Invoke();
                isStasisFreezeApplied = false;
                statusPanel.RemoveStatus(BuffType.StasisFreeze, player);
                break;
        }
    }
    
    private IEnumerator RemoveBuff(BuffType type, Enemy enemy)
    {
        switch (type)
        {
            case BuffType.StasisFreeze:
                yield return new WaitForSeconds(stasysFreezeDuration);
                OnEnemyUnFreeze?.Invoke();
                isStasisFreezeApplied = false;
                statusPanel.RemoveStatus(BuffType.StasisFreeze, enemy);
                break;
        }
    }
}
