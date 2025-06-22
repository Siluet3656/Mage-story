using System;
using System.Collections;
using Data;
using Data.Enums;
using UnityEngine;
using UnityEngine.Serialization;

public class Debuff : MonoBehaviour
{
    [FormerlySerializedAs("statusPanel")]
    [Header("Panel")] 
    [SerializeField] private StatusPanel _statusPanel;
    
    [FormerlySerializedAs("slowDuration")]
    [Header("Debuffs")]
    [SerializeField] private int _slowDuration = 0;
    [FormerlySerializedAs("poisonDuration")]
    [Space]
    [SerializeField] private int _poisonDuration = 0;
    [FormerlySerializedAs("poisonDamage")] [SerializeField] private int _poisonDamage = 0;
    [FormerlySerializedAs("amountOfTicks")] [SerializeField] private int _amountOfTicks = 0;
    [FormerlySerializedAs("critChancePoison")] [SerializeField] private float _critChancePoison;
    [FormerlySerializedAs("critMultyplyPoison")] [SerializeField] private float _critMultyplyPoison;
    [FormerlySerializedAs("firemarkDuration")]
    [Space]
    [SerializeField] private float _firemarkDuration;
    [FormerlySerializedAs("fireMarkBlastPrefub")] [SerializeField] private GameObject _fireMarkBlastPrefub;
    [FormerlySerializedAs("fireMarkBlastDamage")] [SerializeField] private float _fireMarkBlastDamage;
    [FormerlySerializedAs("critChanceFireMark")] [SerializeField] private float _critChanceFireMark;
    [FormerlySerializedAs("critMultyplyFireMark")] [SerializeField] private float _critMultyplyFireMark;

    private bool _isSlowed = false;
    private bool _isPoisoned = false;
    private bool _isFireMarked = false;

    private float _poisonTickRatio;
    private bool _gotcrit = false;

    private void Start()
    {
        _poisonTickRatio = (float)_poisonDuration / (float)_amountOfTicks;
    }

    public void DebuffTarget(DebuffType dtype, Enemy dtarget)
   {
        switch (dtype)
        {
            case DebuffType.Slow:
                if (!_isSlowed)
                {
                    dtarget.SetSpeed(SpeedData.GetDataByType(dtarget.GetSpeed() - 1));
                    _isSlowed = true;
                    _statusPanel.AddStatus(DebuffType.Slow, dtarget);
                    StartCoroutine(RemoveDebuffFromTarget(DebuffType.Slow, dtarget));
                }
                break; 
            case DebuffType.Poison:
                if (!_isPoisoned)
                {
                    _isPoisoned = true;
                    _statusPanel.AddStatus(DebuffType.Poison, dtarget);
                    StartCoroutine(RemoveDebuffFromTarget(DebuffType.Poison, dtarget));
                }
                break;
            case DebuffType.FireMark:
                if (!_isFireMarked)
                {
                    _isFireMarked = true;
                    _statusPanel.AddStatus(DebuffType.FireMark, dtarget);
                    StartCoroutine(WaitForCrit());
                    StartCoroutine(RemoveDebuffFromTarget(DebuffType.FireMark, dtarget));
                }
                break;
        }
   }

   public void GetDebuff(DebuffType dtype, Player dtarget)
   {
        switch (dtype)
        {
            case DebuffType.Slow:

                break;
        }
   }

   public void GotCrit()
   {
       _gotcrit = true;
   }

   private IEnumerator RemoveDebuffFromTarget(DebuffType dtype, Enemy dtarget)
   {
        switch (dtype)
        {
            case DebuffType.Slow:
                yield return new WaitForSeconds(_slowDuration);
                dtarget.SetSpeed(SpeedData.GetDataByType(dtarget.GetSpeed()));
                _isSlowed = false;
                _statusPanel.RemoveStatus(DebuffType.Slow, dtarget);
                break; 
            case DebuffType.Poison:
                for (int i = 0; i < _amountOfTicks; i++)
                {
                    yield return new WaitForSeconds(_poisonTickRatio);
                    dtarget.gameObject.GetComponent<Hp>().TryToTakeCriticalDamage(_poisonDamage, _critMultyplyPoison, _critChancePoison);
                }
                _isPoisoned = false;
                _statusPanel.RemoveStatus(DebuffType.Poison, dtarget);
                break;
            case DebuffType.FireMark:
                yield return new WaitForSeconds(_firemarkDuration);
                StopCoroutine("WaitForCrit");
                _isFireMarked = false;
                _statusPanel.RemoveStatus(DebuffType.FireMark, dtarget);
                break;
        }
   }

   private void RemoveDebuffFromTarget(DebuffType dtype, Player dtarget)
   {
        switch (dtype)
        {
            case DebuffType.Slow:

                break;
        }
   }

   private IEnumerator WaitForCrit()
   {
       while (true)
       {
           yield return new WaitUntil(() => _gotcrit);
           if (_isFireMarked)
           {
               Instantiate(_fireMarkBlastPrefub, this.transform.position, Quaternion.identity).GetComponent<FbBlast>().SetDamage(_fireMarkBlastDamage, _critMultyplyFireMark,_critChanceFireMark);
               FindObjectOfType<Player>().ElementalInvocation(1);
           }
           _gotcrit = false;
       }
   }
}
