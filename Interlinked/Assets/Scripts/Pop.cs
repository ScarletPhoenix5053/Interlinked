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
    private List<SocketedNode> _destinationQueue = new List<SocketedNode>();

    private NodeLink _currentLink;

    private bool _travelling;

    // Properties
    protected SocketedNode Destination { get { return _destinationQueue[0]; } }

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



    public void EndTravel()
    {
        _travelling = false;

        _currentNode = _currentLink.Secondary;
        _currentNode.ContainedPops.Add(this);
        _currentLink = null;
    }
    public void StartTravel()
    {
        _travelling = true;

        _currentNode.ContainedPops.Remove(this);
        _currentNode = null;

        StartCoroutine(AttemptToAttatchRoutine());
    }

    private void MoveToDestination()
    {
        // escape if already travelling
        if (_travelling) { if (_logInfo) Debug.Log("Already travelling, cannot alter course"); return; }

        // check all connections
        foreach (NodeLink connection in _currentNode.Connections)
        {
            if (connection.Secondary == null) continue;
            if (connection.Secondary == Destination)
            {
                _currentLink = connection;
                break;
            }
        }
        
        // exit if cannot find node thru immediate connections
        if (Destination == null || _currentLink == null)
        {
            return;
        }

        // Otherwise start movement
        StartTravel();

        // Adjust destination
        _destinationQueue.Remove(Destination);
    }
    private void UpdateObjective(int currentHour)
    {
        if (_routine == null) { if (_logInfo) Debug.LogError("No routine found, cannot update objective."); return; }
        //if (_travelling) { if (_logInfo) Debug.Log("Already travelling, cannot alter course"); return; }

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
        }
    }

    private IEnumerator AttemptToAttatchRoutine()
    {
        while (!_currentLink.AttatchPop(this)) yield return new WaitForFixedUpdate();
    }
}
