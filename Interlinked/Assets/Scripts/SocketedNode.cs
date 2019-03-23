using UnityEngine;
using System;
using System.Collections.Generic;

public class SocketedNode : MonoBehaviour
{
    // Insepctor vars
    [Header("Level")]
    [SerializeField][Range(1,3)] private int _level = 1;
    [Header("Socket Count")]
    [SerializeField] private int _socketCountLv1 = 2;
    [SerializeField] private int _socketCountLv2 = 4;
    [SerializeField] private int _socketCountLv3 = 8;
    [Header("Capacity")]
    [SerializeField] private int _popCapacityLv1 = 4;
    [SerializeField] private int _popCapacityLv2 = 8;
    [SerializeField] private int _popCapacityLv3 = 16;

    // Private vars
    private const float _nodeSocketSpacing = 0.75f;

    private OverloadedNode _overloadInstance;

    private List<Pop> _containedPops = new List<Pop>();
    private List<Socket> _sockets = new List<Socket>();
    
    // Properties
    protected bool Overloading
    {
        get
        {
            switch (_level)
            {
                case 1: return _containedPops.Count > _popCapacityLv1;
                case 2: return _containedPops.Count > _popCapacityLv2;
                case 3: return _containedPops.Count > _popCapacityLv3;

                default: goto case 1;
            }
        }
    }
    public List<NodeLink> Connections
    {
        get
        {
            var returnList = new List<NodeLink>();

            foreach (Socket socket in _sockets)
            {
                if (!socket.InUse) continue;
                returnList.Add(socket.AttatchedLink);
            }

            return returnList;
        }
    }
    public List<Pop> ContainedPops { get { return _containedPops; } set { _containedPops = value; } }

    private void Awake()
    {
        // Initialize at level 1
        for (int i = 0; i < _socketCountLv1; i++)
        {
            _sockets.Add(Instantiate(Resources.Load<GameObject>("Socket"), transform).GetComponent<Socket>());
        }
        _sockets[0].transform.position += Vector3.up * _nodeSocketSpacing;
        _sockets[0].Parent = this;
        _sockets[1].transform.position += Vector3.down * _nodeSocketSpacing;
        _sockets[1].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        _sockets[1].OffsetDir = Vector3.down;
        _sockets[1].Parent = this;
    }
    private void FixedUpdate()
    {
        // Create overloaded node fx if capacity exceeded
        if (_overloadInstance == null && Overloading)
        {
            _overloadInstance = OverloadedNode.SpawnCongestionZone(this);
        }
        // Remove overload if no longer exceeding capacity
        if (_overloadInstance != null && !Overloading)
        {
            _overloadInstance.RemoveOverloadEffect();
            _overloadInstance = null;
        }
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
            var obj = Resources.Load<GameObject>("Pop");
            var newPopObj = Instantiate(obj, transform.position, obj.transform.rotation, LevelManager.Instance.PopsContainer) as GameObject;
            var newPop = newPopObj.GetComponent<Pop>();
            newPop.Routine = routine;
            newPop.Current = this;

            _containedPops.Add(newPop);
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
