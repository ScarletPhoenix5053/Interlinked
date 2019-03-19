using UnityEngine;
using System.Collections.Generic;

public class Pop : MonoBehaviour
{
    [SerializeField] private bool _logInfo;
    [SerializeField] private Routine _routine;

    [SerializeField] private Node NodeA;
    [SerializeField] private Node NodeB;

    private Node _destination;
    private bool _initialized;

    public bool IsValid { get { return _routine.IsValid && _initialized; } }

    private void Awake()
    {
        _routine = new Routine(new List<RoutineTask>() { new RoutineTask(0, NodeA), new RoutineTask(1, NodeB)});
        TwoPhaseClock.OnHourTick += UpdateObjective;
    }
    private void UpdateObjective(int currentHour)
    {
        var tasks = _routine.GetTasks();
        foreach (RoutineTask task in tasks)
        {
            if (task.Hour == currentHour)
            {
                _destination = task.Destination;
                if (_logInfo) Debug.Log(name + "'s Destination was changed to " + _destination.name);
                transform.position = _destination.transform.position;
                return;
            }
        }
    }
}
