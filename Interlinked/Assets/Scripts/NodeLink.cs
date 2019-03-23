using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeLink : MonoBehaviour
{
    [SerializeField] private Socket _primary;
    [SerializeField] private Socket _secondary;

    private const float _nodeSocketSpacing = 0.75f;
    private const string _linePointTag = "LinePoint";
    private Transform[] _linePoints = new Transform[5];

    public int StartSocket { get; set; }
    public int EndSocket { get; set; }
    public bool IsValid { get; private set; }
    public SocketedNode Primary { get { return _primary.Parent; } }
    public SocketedNode Secondary { get { return _secondary.Parent; } }

    private void Awake()
    {
        InitializeLinePoints();
    }
    private void Reset()
    {
        InitializeLinePoints();
    }
    
    public Vector3[] GetPath()
    {
        List<Vector3> path = new List<Vector3>();
        var lineRenderer = GetComponent<LineRenderer>();
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            path.Add(lineRenderer.GetPosition(i));
        }
        return path.ToArray();
    }
    public SocketedNode GetOther(SocketedNode node)
    {
        if (Primary == node)
        {
            return Secondary;
        }
        else if (Secondary == node)
        {
            return Primary;
        }
        else
        {
            throw new NodeNotContainedException(node + " is not contained inside nodelink " + name);
        }
    }
    public void SetPrimary(Socket primary)
    {
        _primary = primary;

        var pos = primary.transform.position;

        _linePoints[0].position = pos;
        _linePoints[1].position = pos += primary.OffsetDir.normalized * _nodeSocketSpacing;

        UpdateMidPoints();
    }
    public void SetSecondary(Socket secondary)
    {
        _secondary = secondary;

        var pos = secondary.transform.position;

        _linePoints[4].position = pos;
        _linePoints[3].position = pos += secondary.OffsetDir.normalized * _nodeSocketSpacing;

        UpdateMidPoints();
    }
    private void UpdateMidPoints()
    {
        _linePoints[2].position = (_linePoints[1].position + _linePoints[3].position) / 2;        
    }

    public void InitializeLinePoints()
    {
        // Assign child refrences        
        var assignmentIndex = 0;
        foreach (Transform linePoint in transform)
        {
            if (linePoint.tag != _linePointTag) continue;

            _linePoints[assignmentIndex] = linePoint;
            assignmentIndex++;
        }       
        if (assignmentIndex < 5) { Debug.LogError(name + " was unable to find enough children tagged LinePoint: " + assignmentIndex); IsValid = false; return; }
        if (assignmentIndex > 5) { Debug.LogError(name + " found too many children tagged LinePoint: " + assignmentIndex); IsValid = false; return; }
            
        // Return true if validation complete
        IsValid = true;
    }
}
public class NodeNotContainedException : Exception
{
    public NodeNotContainedException()
    {
    }

    public NodeNotContainedException(string message)
        : base(message)
    {
    }

    public NodeNotContainedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}