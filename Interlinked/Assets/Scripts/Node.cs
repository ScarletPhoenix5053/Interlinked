using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private bool _logInfo = false;
    /// <summary>
    /// List of nodes connected to this node from this node.
    /// Connections are one-way.
    /// </summary>
    [SerializeField] private List<Node> _connections;
    /// <summary>
    /// Pops currentley at this node.
    /// </summary>
    [SerializeField] private List<Node> _containedPops;

    /// <summary>
    /// Returns true if connected to the passed node.
    /// </summary>
    /// <param name="otherNode"></param>
    /// <returns></returns>
    public bool IsConnectedTo(Node otherNode)
    {
        if (_connections != null && _connections.Count > 0)
        {
            foreach (Node connectedNode in _connections)
            {
                if (connectedNode == otherNode)
                {
                    return true;
                }                
            }

            // return false if no connection found
            return false;
        }
        return false;
    }
    public bool ConnectTo(Node otherNode)
    {
        // Checks
        if (IsConnectedTo(otherNode)) { if (_logInfo) Debug.LogWarning("Tried to create a connection that already exists"); return false; }
        if (otherNode == this) { if (_logInfo) Debug.LogWarning("Tried to connect to self"); return false; }
        if (_connections == null) _connections = new List<Node>();

        _connections.Add(otherNode);
        return true;
    }
}
