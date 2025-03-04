using System.Collections;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    [Header("Duration")]
    [SerializeField] private int slowDuration = 0;

    private bool isSlowed = false;
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

   public IEnumerator RemoveDebuffFromTarget(DebuffType dtype, Enemy dtarget)
   {
        switch (dtype)
        {
            case DebuffType.Slow:
                yield return new WaitForSeconds(slowDuration);
                isSlowed = false;
                dtarget.SetSpeed(SpeedTypeData.GetDataByID(dtarget.GetSpeed()));
                break; 
        }
   }

   public void RemoveDebuffFromTarget(DebuffType dtype, Player dtarget)
   {
        switch (dtype)
        {
            case DebuffType.Slow:

                break;
        }
   }
}
