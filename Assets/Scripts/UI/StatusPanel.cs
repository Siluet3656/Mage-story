using System.Collections.Generic;
using UnityEngine;

public class StatusPanel : MonoBehaviour
{
   [Header("Debuffs")] 
   [SerializeField] private GameObject slowPF;
   [SerializeField] private GameObject poisonPF;
   [SerializeField] private GameObject firemarkPF;

   [Header("Buffs")] 
   [SerializeField] private GameObject fireauraPF;
   [SerializeField] private GameObject icetombPF;

   private List<GameObject> Statuses = new List<GameObject>();
   private float defaultX = -3.5f;
   private float defaultY = 0.65f;
   private float offset = 1.2f;
   
   private float defaultXbuffs = -9f;
   private float defaultYbuffs = 2.4f;
   private float offsebuffst = 2.8f;
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
         case DebuffType.FireMark:
            gm = Instantiate(firemarkPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultX + offset * Statuses.Count, defaultY, 0);
            Statuses.Add(gm);
            break;
      }
   }

   public void AddStatus(BuffType type)
   {
      GameObject gm;
      switch (type)
      {
         case BuffType.FireAura:
            gm = Instantiate(fireauraPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultXbuffs + offsebuffst * Statuses.Count, defaultYbuffs, 0);
            Statuses.Add(gm);
            break;
         case BuffType.StasisFreeze:
            gm = Instantiate(icetombPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultXbuffs + offsebuffst * Statuses.Count, defaultYbuffs, 0);
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
         case DebuffType.FireMark:
            gm = Statuses.Find(o => o.GetComponent<Status>().dt == DebuffType.FireMark);
            Statuses.Remove(gm);
            Destroy(gm);
            break;
      }
      RefreshStatusesPositions();
   }

   public void RemoveStatus(BuffType type)
   {
      GameObject gm;
      switch (type)
      {
         case BuffType.FireAura:
            gm = Statuses.Find(o => o.GetComponent<Status>().bt == BuffType.FireAura);
            Statuses.Remove(gm);
            Destroy(gm);
            break;
         case BuffType.StasisFreeze:
            gm = Statuses.Find(o => o.GetComponent<Status>().bt == BuffType.StasisFreeze);
            Statuses.Remove(gm);
            Destroy(gm);
            break;
      }
      RefreshStatusesPositionsBuffs();
   }

   private void RefreshStatusesPositions()
   {
      foreach (var status in Statuses)
      {
         status.transform.localPosition = new Vector3(defaultX + offset * Statuses.IndexOf(status), defaultY, 0);
      }
   }
   
   private void RefreshStatusesPositionsBuffs()
   {
      foreach (var status in Statuses)
      {
         status.transform.localPosition = new Vector3(defaultXbuffs + offsebuffst * Statuses.IndexOf(status), defaultYbuffs, 0);
      }
   }
}
