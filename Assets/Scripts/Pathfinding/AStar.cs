using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class AStar : MonoBehaviour
    {
        #region Singletone

        public static AStar Instance;
        AStar()
        {
            if (Instance != null) return;

            Instance = this;
        }

        #endregion

        private readonly List<Node> _nodesOnScene = new List<Node>();
        
        private void Awake()
        {
            RefreshNodesOnScene();
        }

        public void RefreshNodesOnScene()
        {
            _nodesOnScene.Clear();
            _nodesOnScene.AddRange(FindObjectsOfType<Node>());
        }

        public List<Node> NodesOnScene => _nodesOnScene;

        public List<Node> GeneratePath(Node start, Node end)
        {
            List<Node> openSet = new List<Node>();
            
            foreach (Node node in _nodesOnScene)
            {
                node._gScore = float.MaxValue;
            }
            
            start._gScore = 0;
            start._hScore = Vector2.Distance(
                start.transform.position, 
                end.transform.position
                );
            
            openSet.Add(start);

            while (openSet.Count > 0)
            {
                int lowestF = 0;

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FScore() < openSet[lowestF].FScore())
                    {
                        lowestF = i;
                    }
                }

                Node currentNode = openSet[lowestF];
                openSet.Remove(currentNode);

                if (currentNode == end)
                {
                    List<Node> path = new List<Node>();
                    
                    path.Insert(0, end);

                    while (currentNode != start)
                    {
                        currentNode = currentNode._cameFrom;
                        path.Add(currentNode);
                    }
                    
                    path.Reverse();
                    return path;
                }

                foreach (Node connection in currentNode.Connections)
                {
                    float heldGScore = currentNode._gScore + Vector2.Distance(
                        currentNode.transform.position,
                        connection.transform.position
                    );

                    if (heldGScore < connection._gScore)
                    {
                        connection._cameFrom = currentNode;
                        connection._gScore = heldGScore;
                        connection._hScore = Vector2.Distance(
                            connection.transform.position,
                            end.transform.position
                        );

                        if (openSet.Contains(connection) == false)
                        {
                            openSet.Add(connection);
                        }
                    }
                }
            }
            
            return null;
        }
    }
}
