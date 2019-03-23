using UnityEngine;
using System;
using System.Collections.Generic;

public class SocketedNode : MonoBehaviour
{
    // Insepctor vars
    [SerializeField][Range(1,3)] private int _level = 1;
    [SerializeField] private int _socketCountLv1 = 2;
    [SerializeField] private int _socketCountLv2 = 4;
    [SerializeField] private int _socketCountLv3 = 8;

    // Private vars
    private const float _nodeSocketSpacing = 0.75f;
    private List<Pop> _containedPops = new List<Pop>();
    private List<Socket> _sockets = new List<Socket>();
    
    public List<NodeLink> Connections { get; private set; }

    private void Awake()
    {
        // Initialize at level 1
        for (int i = 0; i < _socketCountLv1; i++)
        {
            _sockets.Add(Instantiate(Resources.Load<GameObject>("Socket"), transform).GetComponent<Socket>());
        }
        _sockets[0].transform.position += Vector3.up * _nodeSocketSpacing;
        _sockets[1].transform.position += Vector3.down * _nodeSocketSpacing;
        _sockets[1].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        _sockets[1].OffsetDir = Vector3.down;
    }

    public void IncreaseLevel()
    {
        // Breakout conditions

        // Incriment level

        // Create new sockets
    }
    public void ConnectTo(SocketedNode otherNode, Socket thisSocket, Socket otherSocket)
    {
        // Breakout conditions

        // Spawn link

        // Attatch to sockets

        // Configure and store link
    }
    public void SpawnPops(int count, Routine routine)
    {
        for (int i = 0; i < count; i++)
        {
            var newPopObj = Instantiate(Resources.Load("Pop"), transform.position, Quaternion.identity, LevelManager.Instance.PopsContainer) as GameObject;
            var newPop = newPopObj.GetComponent<Pop>();
            newPop.Routine = routine;
            newPop.Current = this;
        }
    }

    /// <summary>
    /// Returns true if connected to the passed node.
    /// </summary>
    /// <param name="otherNode"></param>
    /// <returns></returns>
    public bool IsConnectedTo(SocketedNode otherNode)
    {
        throw new NotImplementedException();
    }
}
