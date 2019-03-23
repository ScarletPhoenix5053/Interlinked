using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class Routine
{
    public Routine(List<RoutineTask> tasks)
    {
        SetTasks(tasks);
    }

    [SerializeField] private List<RoutineTask> _tasks;

    public bool IsValid { get { return _tasks != null && _tasks.Count > 0; } }    

    public void SetTasks(List<RoutineTask> tasks)
    {
        _tasks = tasks;
    }
    public List<RoutineTask> GetTasks()
    {
        return _tasks;
    }
    public RoutineTask GetTask(int index)
    {
        if (index >= _tasks.Count) throw new RoutineIndexOutOfRangeException();

        return _tasks[index];
    }
}
[Serializable]
public struct RoutineTask
{
    public int Hour;
    public Node Destination;

    public RoutineTask(int hour, Node destination)
    {
        Hour = hour;
        Destination = destination;
    }
}
public class RoutineIndexOutOfRangeException : ArgumentOutOfRangeException
{
    public RoutineIndexOutOfRangeException()
    {
    }

    public RoutineIndexOutOfRangeException(string message)
        : base(message)
    {
    }

    public RoutineIndexOutOfRangeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}