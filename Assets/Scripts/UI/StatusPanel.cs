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
   private float defaultXenemy = -3.5f;
   private float defaultYenemy = 0.65f;
   private float offsetenemy = 1.2f;
   
   private float defaultXplayer = -9f;
   private float defaultYplayer = 2.4f;
   private float offsetBuffsplayer = 2.8f;
   public void AddStatus(DebuffType type, Player player)
   {
      GameObject gm;
      switch (type)
      {
         case DebuffType.Slow:
            gm = Instantiate(slowPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultXplayer + offsetBuffsplayer * Statuses.Count, defaultYplayer, 0);
            Statuses.Add(gm);
            break;
         case DebuffType.Poison:
            gm = Instantiate(poisonPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultXplayer + offsetBuffsplayer * Statuses.Count, defaultYplayer, 0);
            Statuses.Add(gm);
            break;
         case DebuffType.FireMark:
            gm = Instantiate(firemarkPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultXplayer + offsetBuffsplayer * Statuses.Count, defaultYplayer, 0);
            Statuses.Add(gm);
            break;
      }
   }

   public void AddStatus(BuffType type, Player player)
   {
      GameObject gm;
      switch (type)
      {
         case BuffType.FireAura:
            gm = Instantiate(fireauraPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultXplayer + offsetBuffsplayer * Statuses.Count, defaultYplayer, 0);
            Statuses.Add(gm);
            break;
         case BuffType.StasisFreeze:
            gm = Instantiate(icetombPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultXplayer + offsetBuffsplayer * Statuses.Count, defaultYplayer, 0);
            Statuses.Add(gm);
            break;
      }
   }
   
   public void AddStatus(DebuffType type, Enemy enemy)
   {
      GameObject gm;
      switch (type)
      {
         case DebuffType.Slow:
            gm = Instantiate(slowPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultXenemy + offsetenemy * Statuses.Count, defaultYenemy, 0);
            Statuses.Add(gm);
            break;
         case DebuffType.Poison:
            gm = Instantiate(poisonPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultXenemy + offsetenemy * Statuses.Count, defaultYenemy, 0);
            Statuses.Add(gm);
            break;
         case DebuffType.FireMark:
            gm = Instantiate(firemarkPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultXenemy + offsetenemy * Statuses.Count, defaultYenemy, 0);
            Statuses.Add(gm);
            break;
      }
   }
   
   public void AddStatus(BuffType type, Enemy enemy)
   {
      GameObject gm;
      switch (type)
      {
         case BuffType.FireAura:
            gm = Instantiate(fireauraPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultXenemy + offsetenemy * Statuses.Count, defaultYenemy, 0);
            Statuses.Add(gm);
            break;
         case BuffType.StasisFreeze:
            gm = Instantiate(icetombPF, this.gameObject.transform, false);
            gm.transform.localPosition = new Vector3(defaultXenemy + offsetenemy * Statuses.Count, defaultYenemy, 0);
            Statuses.Add(gm);
            break;
      }
   }

   public void RemoveStatus(DebuffType type, Player player)
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
      RefreshStatusesPositions(player);
   }

   public void RemoveStatus(BuffType type, Player player)
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
      RefreshStatusesPositions(player);
   }
   
   public void RemoveStatus(DebuffType type, Enemy enemy)
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
      RefreshStatusesPositions(enemy);
   }

   public void RemoveStatus(BuffType type, Enemy enemy)
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
      RefreshStatusesPositions(enemy);
   }

   private void RefreshStatusesPositions(Player player)
   {
      foreach (var status in Statuses)
      {
         status.transform.localPosition = new Vector3(defaultXplayer + offsetBuffsplayer * Statuses.IndexOf(status), defaultYplayer, 0);
      }
   }
   
   private void RefreshStatusesPositions(Enemy enemy)
   {
      foreach (var status in Statuses)
      {
         status.transform.localPosition = new Vector3(defaultXenemy + offsetenemy * Statuses.IndexOf(status), defaultYenemy, 0);
      }
   }
}
