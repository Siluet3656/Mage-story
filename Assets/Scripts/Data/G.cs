using System.Collections.Generic;
using UnityEngine;
using EntityStaff;
using Pathfinding;
using PlayerStaff;
using Shard;

namespace Data
{
    public static class G
    {
        public static FootprintPool FootprintPool;
        public static readonly List<Node> NodesOnScene = new List<Node>();
        public static readonly List<GameObject> ActiveFootprints = new List<GameObject>();
        
        public static Hp PlayersHp;
        public static Player Player;
        public static PlayersShard PlayersShard;
    }
}
