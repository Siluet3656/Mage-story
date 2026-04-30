using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeMap
{
    public enum NodeType
    {
        Battle,
        Elite,
        Rest,
        Event,
        Boss
    }

    [Serializable]
    public class MapNodeData
    {
        public NodeType Type;
        public int ColumnIndex;
        public int RowIndex;
        public List<int> ConnectedNodeIndices = new List<int>();
        public bool IsCompleted;
        public bool IsUnlocked;
    }
}
