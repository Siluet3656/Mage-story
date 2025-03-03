using UnityEngine;

public class Debuff : MonoBehaviour
{
   public void DebuffTarget(DebuffType dtype, Enemy dtarget)
   {
        switch (dtype)
        {
            case DebuffType.Slow:
                dtarget.SetSpeed(dtarget.GetSpeed()/2);
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
}
