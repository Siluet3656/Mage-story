using System.Collections.Generic;
using Data.Enums;
using Statuses;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
   public class StatusPanel : MonoBehaviour
   {
      [FormerlySerializedAs("slowPF")]
      [Header("Debuffs")] 
      [SerializeField] private GameObject _slowPf;
      [FormerlySerializedAs("poisonPF")] [SerializeField] private GameObject _poisonPf;
      [FormerlySerializedAs("firemarkPF")] [SerializeField] private GameObject _firemarkPf;

      [FormerlySerializedAs("fireauraPF")]
      [Header("Buffs")] 
      [SerializeField] private GameObject _fireauraPf;
      [FormerlySerializedAs("icetombPF")] [SerializeField] private GameObject _icetombPf;

      private List<GameObject> _statuses = new List<GameObject>();
      private float _defaultXenemy = -3.5f;
      private float _defaultYenemy = 0.65f;
      private float _offsetenemy = 1.2f;
   
      private float _defaultXplayer = -9f;
      private float _defaultYplayer = 2.4f;
      private float _offsetBuffsplayer = 2.8f;
      public void AddStatus(DebuffType type, Player player)
      {
         GameObject gm;
         switch (type)
         {
            case DebuffType.Slow:
               gm = Instantiate(_slowPf, this.gameObject.transform, false);
               gm.transform.localPosition = new Vector3(_defaultXplayer + _offsetBuffsplayer * _statuses.Count, _defaultYplayer, 0);
               _statuses.Add(gm);
               break;
            case DebuffType.Poison:
               gm = Instantiate(_poisonPf, this.gameObject.transform, false);
               gm.transform.localPosition = new Vector3(_defaultXplayer + _offsetBuffsplayer * _statuses.Count, _defaultYplayer, 0);
               _statuses.Add(gm);
               break;
            case DebuffType.FireMark:
               gm = Instantiate(_firemarkPf, this.gameObject.transform, false);
               gm.transform.localPosition = new Vector3(_defaultXplayer + _offsetBuffsplayer * _statuses.Count, _defaultYplayer, 0);
               _statuses.Add(gm);
               break;
         }
      }

      public void AddStatus(BuffType type, Player player)
      {
         GameObject gm;
         switch (type)
         {
            case BuffType.FireAura:
               gm = Instantiate(_fireauraPf, this.gameObject.transform, false);
               gm.transform.localPosition = new Vector3(_defaultXplayer + _offsetBuffsplayer * _statuses.Count, _defaultYplayer, 0);
               _statuses.Add(gm);
               break;
            case BuffType.StasisFreeze:
               gm = Instantiate(_icetombPf, this.gameObject.transform, false);
               gm.transform.localPosition = new Vector3(_defaultXplayer + _offsetBuffsplayer * _statuses.Count, _defaultYplayer, 0);
               _statuses.Add(gm);
               break;
         }
      }
   
      public void AddStatus(DebuffType type, Enemy enemy)
      {
         GameObject gm;
         switch (type)
         {
            case DebuffType.Slow:
               gm = Instantiate(_slowPf, this.gameObject.transform, false);
               gm.transform.localPosition = new Vector3(_defaultXenemy + _offsetenemy * _statuses.Count, _defaultYenemy, 0);
               _statuses.Add(gm);
               break;
            case DebuffType.Poison:
               gm = Instantiate(_poisonPf, this.gameObject.transform, false);
               gm.transform.localPosition = new Vector3(_defaultXenemy + _offsetenemy * _statuses.Count, _defaultYenemy, 0);
               _statuses.Add(gm);
               break;
            case DebuffType.FireMark:
               gm = Instantiate(_firemarkPf, this.gameObject.transform, false);
               gm.transform.localPosition = new Vector3(_defaultXenemy + _offsetenemy * _statuses.Count, _defaultYenemy, 0);
               _statuses.Add(gm);
               break;
         }
      }
   
      public void AddStatus(BuffType type, Enemy enemy)
      {
         GameObject gm;
         switch (type)
         {
            case BuffType.FireAura:
               gm = Instantiate(_fireauraPf, this.gameObject.transform, false);
               gm.transform.localPosition = new Vector3(_defaultXenemy + _offsetenemy * _statuses.Count, _defaultYenemy, 0);
               _statuses.Add(gm);
               break;
            case BuffType.StasisFreeze:
               gm = Instantiate(_icetombPf, this.gameObject.transform, false);
               gm.transform.localPosition = new Vector3(_defaultXenemy + _offsetenemy * _statuses.Count, _defaultYenemy, 0);
               _statuses.Add(gm);
               break;
         }
      }

      public void RemoveStatus(DebuffType type, Player player)
      {
         GameObject gm;
         switch (type)
         {
            case DebuffType.Slow:
               gm = _statuses.Find(o => o.GetComponent<Status>()._dt == DebuffType.Slow);
               _statuses.Remove(gm);
               Destroy(gm);
               break;
            case DebuffType.Poison:
               gm = _statuses.Find(o => o.GetComponent<Status>()._dt == DebuffType.Poison);
               _statuses.Remove(gm);
               Destroy(gm);
               break;
            case DebuffType.FireMark:
               gm = _statuses.Find(o => o.GetComponent<Status>()._dt == DebuffType.FireMark);
               _statuses.Remove(gm);
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
               gm = _statuses.Find(o => o.GetComponent<Status>()._bt == BuffType.FireAura);
               _statuses.Remove(gm);
               Destroy(gm);
               break;
            case BuffType.StasisFreeze:
               gm = _statuses.Find(o => o.GetComponent<Status>()._bt == BuffType.StasisFreeze);
               _statuses.Remove(gm);
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
               gm = _statuses.Find(o => o.GetComponent<Status>()._dt == DebuffType.Slow);
               _statuses.Remove(gm);
               Destroy(gm);
               break;
            case DebuffType.Poison:
               gm = _statuses.Find(o => o.GetComponent<Status>()._dt == DebuffType.Poison);
               _statuses.Remove(gm);
               Destroy(gm);
               break;
            case DebuffType.FireMark:
               gm = _statuses.Find(o => o.GetComponent<Status>()._dt == DebuffType.FireMark);
               _statuses.Remove(gm);
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
               gm = _statuses.Find(o => o.GetComponent<Status>()._bt == BuffType.FireAura);
               _statuses.Remove(gm);
               Destroy(gm);
               break;
            case BuffType.StasisFreeze:
               gm = _statuses.Find(o => o.GetComponent<Status>()._bt == BuffType.StasisFreeze);
               _statuses.Remove(gm);
               Destroy(gm);
               break;
         }
         RefreshStatusesPositions(enemy);
      }

      private void RefreshStatusesPositions(Player player)
      {
         foreach (var status in _statuses)
         {
            status.transform.localPosition = new Vector3(_defaultXplayer + _offsetBuffsplayer * _statuses.IndexOf(status), _defaultYplayer, 0);
         }
      }
   
      private void RefreshStatusesPositions(Enemy enemy)
      {
         foreach (var status in _statuses)
         {
            status.transform.localPosition = new Vector3(_defaultXenemy + _offsetenemy * _statuses.IndexOf(status), _defaultYenemy, 0);
         }
      }
   }
}
