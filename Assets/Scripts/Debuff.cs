using UnityEngine;

public class Debuff : MonoBehaviour
{
    [Header("Duration")]
    [SerializeField] private int SlowDuration = 0;

   public void DebuffTarget(DebuffType dtype, Enemy dtarget)
   {
        switch (dtype)
        {
            case DebuffType.Slow:
                dtarget.SetSpeed((int)dtarget.GetSpeed()/2);
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

   public void RemoveDebuffFromTarget(DebuffType dtype, Enemy dtarget)
   {
        switch (dtype)
        {
            case DebuffType.Slow:
                
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
