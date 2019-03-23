using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pop : MonoBehaviour
{
    [SerializeField] private bool _logInfo;
    [SerializeField] private Routine _routine;

    [SerializeField] private SocketedNode _nodeA;
    [SerializeField] private SocketedNode _nodeB;

    [SerializeField] private float _moveSpeed;

    private SocketedNode _currentNode;
    private SocketedNode _destination;
    private NodeLink _currentLink;

    private bool _travelling;

    public bool Obstructed { get; private set; }

    public Routine Routine { get { return _routine; } set { _routine = value; } }
    public SocketedNode Current { get { return _currentNode; } set { _currentNode = value; } }

    private void Awake()
    {
        TwoPhaseClock.OnHourTick += UpdateObjective;
    }

    private void MoveToDestination()
    {
        // exit if cannot find node thru immediate connections
        if (_destination == null)
        {
            // create obstruction instance
            return;
        }

        // Find link to dest node
        // check all connections
        foreach (NodeLink connection in _currentNode.Connections)
        {
            if (connection.Secondary == null) continue;
            Debug.Log("looped");
            if (connection.Secondary == _destination)
            {
                _currentLink = connection;
                break;
            }
        }
        if (_currentLink == null) Debug.LogError("No link");

        // Otherwise start movement
        _travelling = true;
        StartCoroutine(GoToDestinationRoutine());
    }
    private void UpdateObjective(int currentHour)
    {
        if (_routine == null) { if (_logInfo) Debug.LogError("No routine found, cannot update objective."); return; }
        if (_travelling) { if (_logInfo) Debug.Log("Already travelling, cannot alter course"); return; }

        var tasks = _routine.GetTasks();
        foreach (RoutineTask task in tasks)
        {
            if (task.Hour == currentHour)
            {
                _destination = task.Destination;
                if (_logInfo && _destination != null) Debug.Log(name + "'s Destination was changed to " + _destination.name);
                MoveToDestination();
                return;
            }
        }
    }

    private IEnumerator GoToDestinationRoutine()
    {
        // Get path
        Debug.Assert(_currentLink != null);
        var path = _currentLink.GetPath();

        // Place pop on path
        transform.position = path[0];
        transform.LookAt(path[1]);

        // Travel along path
        for (int i = 0; i < path.Length -1; i++)
        {
            transform.position = path[i];
            transform.LookAt(path[i+1], Vector3.back);

            yield return new WaitForSeconds(0.06f);
        }

        // At end of path place pop inside node
        _currentNode = _currentLink.Secondary;
        transform.position = _currentNode.transform.position;
        _travelling = false;
    }
}
