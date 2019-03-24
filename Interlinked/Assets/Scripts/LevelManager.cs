using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    // Inspector vars
    [SerializeField] private Transform _popsContainter;
    [SerializeField] private Transform _nodesContainter;
    [Header("Generation")]
    [SerializeField] private Vector2 _nodeGenerationSpace = new Vector2(6,4);
    [SerializeField] private Routine _routineA;
    [SerializeField] private Routine _routineB;
    [SerializeField] private Routine _routineC;
    [SerializeField] private float _growthCycleTime = 5f;
    [SerializeField] private int _newPopsOnCycle = 5;
    [SerializeField] private int _newNodeEveryNCycles = 2;
    [Header("Resource")]
    [SerializeField] private int _resource = 100;
    [SerializeField] private int _resourceGain = 1;
    [SerializeField] private int _resourceDrainMultiplier = 3;

    // Private fields
    private List<SocketedNode> _nodes = new List<SocketedNode>();
    private float _growthCycleTimer = 0;
    private int _cycleN = 0;
    private System.Random _random = new System.Random();

    // Properties
    public static LevelManager Instance { get; private set; }

    public Transform PopsContainer { get { return _popsContainter; } }

    public int Resource { get { return _resource; } }
    public int ResourceDrainMultiplier { get { return _resourceDrainMultiplier; } }

    // Events
    public static event TrafficOverloadEventHandler OnTrafficOverload;
    
    // Runtime
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);

        // Assign initial nodes
        if (_nodesContainter != null)
        {
            foreach (Transform child in _nodesContainter)
            {
                SocketedNode node;
                if (node = child.GetComponent<SocketedNode>())
                {
                    _nodes.Add(node);
                }
            }
        }
        
        // Spawn pops
        _nodes[0].SpawnPops(5, _routineA);
        
        // Config events
        OnTrafficOverload += RestartGame;
    }
    private void FixedUpdate()
    {
        // incriment timer
        _growthCycleTimer += Time.fixedDeltaTime;

        // do growth cycle 
        if (_growthCycleTimer >= _growthCycleTime)
        {
            Debug.Log("GROW");
            _growthCycleTimer = 0;
            _cycleN++;

            // Generate random routine
            var task1 = new RoutineTask(0, GetRandomNode());
            var task2 = new RoutineTask(1, GetRandomNode());
            var randomRoutine = new Routine(new List<RoutineTask>() { task1, task2 });

            // Spawn pops
            GetRandomNode().SpawnPops(_newPopsOnCycle, randomRoutine);



            // add node on correct cycles
            if (_cycleN >= _newNodeEveryNCycles)
            {
                Debug.Log("NEWNODE");
                _cycleN = 0;

                // Create random pos
                var randomPos = new Vector2();
                var randX = (float)_random.NextDouble() * _nodeGenerationSpace.x;
                var randY = (float)_random.NextDouble() * _nodeGenerationSpace.y;
                randomPos.x = _random.NextDouble() < 0.5f ? -randX : +randX ;
                randomPos.y = _random.NextDouble() < 0.5f ? -randY : +randY;

                // Spawn new node
                var newNode = Instantiate(Resources.Load<GameObject>("Node"), randomPos, Quaternion.identity, _nodesContainter);
                newNode.name = "Node " + _nodesContainter.childCount;
            }
        }
    }

    public static void TiggerTraficOverload()
    {
        OnTrafficOverload?.Invoke();
    }
    public static void IncrimentResource()
    {
        Instance._resource += Instance._resourceGain;
    }
    public bool UseResource(int amount)
    {
        if (amount < _resource)
        {
            _resource -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    private SocketedNode GetRandomNode()
    {
        var targetNodeIndex = _random.Next(1, _nodesContainter.childCount);
        var targetNode = _nodesContainter.GetChild(targetNodeIndex).GetComponent<SocketedNode>();
        return targetNode;
    }
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
public delegate void TrafficOverloadEventHandler();