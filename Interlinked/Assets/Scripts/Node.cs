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
    [SerializeField] private List<NodeLink> _connections;
    /// <summary>
    /// Pops currentley at this node.
    /// </summary>
    [SerializeField] private List<Pop> _containedPops;

    [SerializeField] private int _level = 1;

    private const int _connectionsMaxLv1 = 2;
    private const int _connectionsMaxLv2 = 4;
    private const int _connectionsMaxLv3 = 8;
    private int _newConnectionIndex = 0;

    public int NCI { get { return _newConnectionIndex; } }
    public int MaxConnections
    {
        get
        {
            switch (_level)
            {
                case 1: return _connectionsMaxLv1;
                case 2: return _connectionsMaxLv2;
                case 3: return _connectionsMaxLv3;

                default: goto case 1;
            }
        }
    }
    public List<NodeLink> Connections { get { return _connections; } }

    /// <summary>
    /// Returns true if connected to the passed node.
    /// </summary>
    /// <param name="otherNode"></param>
    /// <returns></returns>
    public bool IsConnectedTo(Node otherNode)
    {
        if (_connections != null && _connections.Count > 0)
        {
            foreach (NodeLink connection in _connections)
            {
                if (connection.GetOther(this) == otherNode)
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
        if (_connections.Count >= MaxConnections) { if (_logInfo) Debug.LogWarning(name + " has used all of its connections."); }
        if (IsConnectedTo(otherNode)) { if (_logInfo) Debug.LogWarning("Tried to create a connection that already exists"); return false; }
        if (otherNode == this) { if (_logInfo) Debug.LogWarning("Tried to connect to self"); return false; }        
        if (_connections == null) _connections = new List<NodeLink>();

        // Spawn new link
        var newConnection = (Instantiate(Resources.Load("NodeLink"), transform) as GameObject).GetComponent<NodeLink>();

        // Set socket of connection to place in list
        SocketStart(ref newConnection);
        otherNode.SocketEnd(ref newConnection);

        // Configure and store new link
        newConnection.SetPrimary(this);
        newConnection.SetSecondary(otherNode);
        _connections.Add(newConnection);


        return true;
    }
    public void SocketStart(ref NodeLink connection)
    {
        connection.StartSocket = _newConnectionIndex;
        _newConnectionIndex++;
    }
    public void SocketEnd(ref NodeLink connection)
    {
        connection.EndSocket = _newConnectionIndex;
        _newConnectionIndex++;
    }

    public void SpawnPops(int count, Routine routine)
    {
        for (int i = 0 ; i < count; i++)
        {
            var newPopObj = Instantiate(Resources.Load("Pop"), transform.position, Quaternion.identity, LevelManager.Instance.PopsContainer) as GameObject;
            var newPop = newPopObj.GetComponent<Pop>();
            newPop.Routine = routine;
            newPop.Current = this;
        }
    }
}