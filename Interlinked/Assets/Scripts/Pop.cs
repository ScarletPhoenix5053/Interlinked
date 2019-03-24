using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pop : MonoBehaviour
{
    // Inspector
    [SerializeField] private bool _logInfo;
    [SerializeField] private Routine _routine;
    [SerializeField] private float _stepDelay = 0.06f;

    private SocketedNode _currentNode;

    private SocketedNode _currentDestination;
    private List<SocketedNode> _destinationQueue = new List<SocketedNode>();

    private NodeLink _currentLink;
    private List<NodeLink> _linkQueue = new List<NodeLink>();

    private bool _travelling;
    
    // Propertie
    public bool LogInfo { get { return _logInfo; } set { _logInfo = value; } }
    public Routine Routine { get { return _routine; } set { _routine = value; } }
    public SocketedNode Current { get { return _currentNode; } set { _currentNode = value; } }
    public int CurrentLineSection { get; set; }

    private void Awake()
    {
        TwoPhaseClock.OnHourTick += UpdateObjective;
    }
    private void OnDestroy()
    {
        TwoPhaseClock.OnHourTick -= UpdateObjective;
    }
    private void FixedUpdate()
    {
        TryMoveToDestination();
    }


    public void EndTravel()
    {
        _travelling = false;
        if (_logInfo) Debug.Log("Ending a travel");

        _currentNode = _currentLink.Secondary;
        _currentLink = null;
        
        _currentNode.ContainedPops.Add(this);
    }
    public void StartTravel()
    {
        _travelling = true;
        if (_logInfo) Debug.Log("Starting a travel");

        _currentNode.ContainedPops.Remove(this);

        // Adjust destination
        _destinationQueue.Remove(_currentDestination);
        UpdateDestination();
    }

    private void TryMoveToDestination()
    {
        // escape if already travelling
        if (_travelling) { if (_logInfo) /*Debug.Log("Already travelling, cannot alter course"); */return; }
        
        if (_currentDestination == _currentNode)
        {
            if (_logInfo) Debug.Log("Purging a destination because " + name + " is already there");
            _destinationQueue.Remove(_currentDestination);
            UpdateDestination();
        }

        // Find path to new destination
        foreach (NodeLink connection in _currentNode.Connections)
        {
            if (connection.Secondary == null) continue;
            if (connection.Secondary == _currentDestination)
            {
                _currentLink = connection;
                break;
            }
        }

        // exit if cannot find node thru immediate connections
        if (_currentDestination == null || _currentLink == null)
        {
            return;
        }

        // Otherwise start movement
        if (_currentLink.AttatchPop(this)) StartTravel();

    }
    private void UpdateObjective(int currentHour)
    {
        if (_routine == null) { if (_logInfo) Debug.LogError("No routine found, cannot update objective."); return; }
        //if (_travelling) { if (_logInfo) Debug.Log("Already travelling, cannot alter course"); return; }

        // Find new destination
        SocketedNode newDest = null;
        foreach (RoutineTask task in _routine.GetTasks())
        {
            if (task.Hour == currentHour)
            {
                newDest = task.Destination;
            }
        }

        // Break if no new dest on this update cycle
        if (newDest == null) return;

        // Find first path to finaldest
        List<SocketedNode> pathToFinalDest = null;
        var pathIsOneStep = false;
        foreach (NodeLink connection in _currentNode.Connections)
        {
            if (connection.Secondary == null) continue;
            if (connection.Secondary == newDest)
            {
                pathIsOneStep = true;
                break;
            }
        }

        // Add nodes to destination queue in order
        if (pathIsOneStep && !_destinationQueue.Contains(newDest)) _destinationQueue.Add(newDest);
        UpdateDestination();

        /*
        // log destination-queue
        if (_logInfo) Debug.Log(_destinationQueue.Count);

        var tasks = _routine.GetTasks();
        foreach (RoutineTask task in tasks)
        {
            if (task.Hour == currentHour)
            {
                _destinationQueue.Add(task.Destination);
                if (_logInfo && Destination != null) Debug.Log(name + "'s Destination was changed to " + Destination.name);
                MoveToDestination();
                return;
            }
        }*/
    }
    
    private void UpdateDestination()
    {
        if (_destinationQueue.Count > 0)
        {
            _currentDestination = _destinationQueue[0];
            if (_logInfo)
            {
                Debug.Log(name + "'s Destination was changed to " + _currentDestination.name);
                Debug.Log(name + "'s destination queue now has " + _destinationQueue.Count + " entries");
            }
        }
        else
        {
            _currentDestination = null;
        }
    }
}
