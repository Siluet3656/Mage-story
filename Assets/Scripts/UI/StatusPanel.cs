using System.Collections.Generic;
using UnityEngine;

public class StatusPanel : MonoBehaviour
{
   [Header("Debuffs")] 
   [SerializeField] private GameObject slowPF;
   [SerializeField] private GameObject poisonPF;

   private List<GameObject> Statuses = new List<GameObject>();
   private float defaultX = -3.5f;
   private float defaultY = 0.65f;
   private float offset = 1.2f;
   public void AddStatus(DebuffType type)
   {
      GameObject gm;
      switch (type)
      {
         case DebuffType.Slow:
            gm = Instantiate(slowPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultX + offset * Statuses.Count, defaultY, 0);
            Statuses.Add(gm);
            break;
         case DebuffType.Poison:
            gm = Instantiate(poisonPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultX + offset * Statuses.Count, defaultY, 0);
            Statuses.Add(gm);
            break;
      }
   }

   public void RemoveStatus(DebuffType type)
   {
      GameObject gm;
      switch (type)
      {
         case DebuffType.Slow:
            gm = Statuses.Find(o => o.GetComponent<Status>().dt == DebuffType.Slow);
            Statuses.Remove(gm);
            Destroy(gm);
            break;
         case DebuffType.Poison:
            gm = Statuses.Find(o => o.GetComponent<Status>().dt == DebuffType.Poison);
            Statuses.Remove(gm);
            Destroy(gm);
            break;
      }
      RefreshStatusesPositions();
   }

   private void RefreshStatusesPositions()
   {
      foreach (var status in Statuses)
      {
         status.transform.localPosition = new Vector3(defaultX + offset * Statuses.IndexOf(status), defaultY, 0);
      }
   }
}
