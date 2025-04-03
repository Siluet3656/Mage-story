using System;
using System.Collections;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    [Header("Panel")] 
    [SerializeField] private StatusPanel statusPanel;
    
    [Header("Debuffs")]
    [SerializeField] private int slowDuration = 0;
    [Space]
    [SerializeField] private int poisonDuration = 0;
    [SerializeField] private int poisonDamage = 0;
    [SerializeField] private int amountOfTicks = 0;
    [SerializeField] private float critChancePoison;
    [SerializeField] private float critMultyplyPoison;
    [Space]
    [SerializeField] private float firemarkDuration;
    [SerializeField] private GameObject fireMarkBlastPrefub;
    [SerializeField] private float fireMarkBlastDamage;
    [SerializeField] private float critChanceFireMark;
    [SerializeField] private float critMultyplyFireMark;

    private bool isSlowed = false;
    private bool isPoisoned = false;
    private bool isFireMarked = false;

    private float poisonTickRatio;
    private bool _gotcrit = false;

    private void Start()
    {
        poisonTickRatio = (float)poisonDuration / (float)amountOfTicks;
    }

    public void DebuffTarget(DebuffType dtype, Enemy dtarget)
   {
        switch (dtype)
        {
            case DebuffType.Slow:
                if (!isSlowed)
                {
                    dtarget.SetSpeed(SpeedTypeData.GetDataByID(dtarget.GetSpeed() - 1));
                    isSlowed = true;
                    statusPanel.AddStatus(DebuffType.Slow, dtarget);
                    StartCoroutine(RemoveDebuffFromTarget(DebuffType.Slow, dtarget));
                }
                break; 
            case DebuffType.Poison:
                if (!isPoisoned)
                {
                    isPoisoned = true;
                    statusPanel.AddStatus(DebuffType.Poison, dtarget);
                    StartCoroutine(RemoveDebuffFromTarget(DebuffType.Poison, dtarget));
                }
                break;
            case DebuffType.FireMark:
                if (!isFireMarked)
                {
                    isFireMarked = true;
                    statusPanel.AddStatus(DebuffType.FireMark, dtarget);
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
                yield return new WaitForSeconds(slowDuration);
                dtarget.SetSpeed(SpeedTypeData.GetDataByID(dtarget.GetSpeed()));
                isSlowed = false;
                statusPanel.RemoveStatus(DebuffType.Slow, dtarget);
                break; 
            case DebuffType.Poison:
                for (int i = 0; i < amountOfTicks; i++)
                {
                    yield return new WaitForSeconds(poisonTickRatio);
                    dtarget.gameObject.GetComponent<HP>().TryToTakeCriticalDamage(poisonDamage, critMultyplyPoison, critChancePoison);
                }
                isPoisoned = false;
                statusPanel.RemoveStatus(DebuffType.Poison, dtarget);
                break;
            case DebuffType.FireMark:
                yield return new WaitForSeconds(firemarkDuration);
                StopCoroutine("WaitForCrit");
                isFireMarked = false;
                statusPanel.RemoveStatus(DebuffType.FireMark, dtarget);
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
           if (isFireMarked)
           {
               Instantiate(fireMarkBlastPrefub, this.transform.position, Quaternion.identity).GetComponent<FB_blast>().SetDamage(fireMarkBlastDamage, critMultyplyFireMark,critChanceFireMark);
               FindObjectOfType<Player>().ElementalInvocation(1);
           }
           _gotcrit = false;
       }
   }
}
