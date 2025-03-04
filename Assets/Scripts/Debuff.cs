using System;
using System.Collections;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    [Header("Debuffs")]
    [SerializeField] private int slowDuration = 0;
    [Space]
    [SerializeField] private int poisonDuration = 0;
    [SerializeField] private int poisonDamage = 0;
    [SerializeField] private int amountOfTicks = 0;

    private bool isSlowed = false;
    private bool isPoisoned = false;

    private float poisonTickRatio;

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
                    StartCoroutine(RemoveDebuffFromTarget(DebuffType.Slow, dtarget));
                }
                break; 
            case DebuffType.Poison:
                if (!isPoisoned)
                {
                    isPoisoned = true;
                    StartCoroutine(RemoveDebuffFromTarget(DebuffType.Poison, dtarget));
                }
                break;
        }
   }

   public void DebuffTarget(DebuffType dtype, Player dtarget)
   {
        switch (dtype)
        {
            case DebuffType.Slow:

                break;
        }
   }

   private IEnumerator RemoveDebuffFromTarget(DebuffType dtype, Enemy dtarget)
   {
        switch (dtype)
        {
            case DebuffType.Slow:
                yield return new WaitForSeconds(slowDuration);
                isSlowed = false;
                dtarget.SetSpeed(SpeedTypeData.GetDataByID(dtarget.GetSpeed()));
                break; 
            case DebuffType.Poison:
                for (int i = 0; i < amountOfTicks; i++)
                {
                    yield return new WaitForSeconds(poisonTickRatio);
                    dtarget.gameObject.GetComponent<HP>().TakeDamage(poisonDamage);
                }
                isPoisoned = false;
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
}
