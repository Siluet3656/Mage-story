using System.Collections;
using Data.Enums;
using UnityEngine;
using UnityEngine.Serialization;

public class Buff : MonoBehaviour
{
    [FormerlySerializedAs("statusPanel")]
    [Header("Panel")] 
    [SerializeField] private StatusPanel _statusPanel;
    
    [FormerlySerializedAs("fireAuraDuration")]
    [Header("Buffs")]
    [SerializeField] private int _fireAuraDuration;
    [FormerlySerializedAs("fireAuraMultAdjust")] [SerializeField] private float _fireAuraMultAdjust;
    [FormerlySerializedAs("fireAuraChanceAdjust")] [SerializeField] private float _fireAuraChanceAdjust;
    [FormerlySerializedAs("stasysFreezeDuration")]
    [Space] 
    [SerializeField] private int _stasysFreezeDuration;
    
    private bool _isFireAuraApplied = false;
    private bool _isStasisFreezeApplied = false;

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
                if (!_isFireAuraApplied)
                {
                    player.SetCritAdjustFire(_fireAuraMultAdjust,_fireAuraChanceAdjust);
                    _isFireAuraApplied = true;
                    _statusPanel.AddStatus(BuffType.FireAura, player);
                    StartCoroutine(RemoveBuff(BuffType.FireAura, player));
                }
                break;
            case BuffType.StasisFreeze:
                if (!_isStasisFreezeApplied)
                {
                    OnPlayerFreeze?.Invoke();
                    _isStasisFreezeApplied = true;
                    _statusPanel.AddStatus(BuffType.StasisFreeze, player);
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
                if (!_isStasisFreezeApplied)
                {
                    OnEnemyFreeze?.Invoke();
                    _isStasisFreezeApplied = true;
                    _statusPanel.AddStatus(BuffType.StasisFreeze, enemy);
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
                yield return new WaitForSeconds(_fireAuraDuration);
                player.SetCritAdjustFire(1,1);
                _isFireAuraApplied = false;
                _statusPanel.RemoveStatus(BuffType.FireAura, player);
                break;
            case BuffType.StasisFreeze:
                yield return new WaitForSeconds(_stasysFreezeDuration);
                OnPlayerUnFreeze?.Invoke();
                _isStasisFreezeApplied = false;
                _statusPanel.RemoveStatus(BuffType.StasisFreeze, player);
                break;
        }
    }
    
    private IEnumerator RemoveBuff(BuffType type, Enemy enemy)
    {
        switch (type)
        {
            case BuffType.StasisFreeze:
                yield return new WaitForSeconds(_stasysFreezeDuration);
                OnEnemyUnFreeze?.Invoke();
                _isStasisFreezeApplied = false;
                _statusPanel.RemoveStatus(BuffType.StasisFreeze, enemy);
                break;
        }
    }
}
