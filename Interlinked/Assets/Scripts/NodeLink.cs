using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeLink : MonoBehaviour
{
    [SerializeField] private Node _primary;
    [SerializeField] private Node _secondary;

    private const float _nodeSocketSpacing = 0.75f;
    private const string _linePointTag = "LinePoint";
    private Transform[] _linePoints = new Transform[5];

    public int StartSocket { get; set; }
    public int EndSocket { get; set; }
    public bool IsValid { get; private set; }
    public Node Primary { get { return _primary; } }
    public Node Secondary { get { return _secondary; } }

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
    public Node GetOther(Node node)
    {
        if (_primary == node)
        {
            return _secondary;
        }
        else if (_secondary == node)
        {
            return _primary;
        }
        else
        {
            throw new NodeNotContainedException(node + " is not contained inside nodelink " + name);
        }
    }
    public void SetPrimary(Node primary)
    {
        _primary = primary;

        var pos = primary.transform.position;

        switch (StartSocket)
        {
            case 0:
                _linePoints[0].position = pos += Vector3.up * _nodeSocketSpacing;
                _linePoints[1].position = pos += Vector3.up * _nodeSocketSpacing;
                break;

            case 1:
                _linePoints[0].position = pos += Vector3.right * _nodeSocketSpacing;
                _linePoints[1].position = pos += Vector3.right * _nodeSocketSpacing;
                break;

            case 2:
                _linePoints[0].position = pos += Vector3.down * _nodeSocketSpacing;
                _linePoints[1].position = pos += Vector3.down * _nodeSocketSpacing;
                break;

            case 3:
                _linePoints[0].position = pos += Vector3.left * _nodeSocketSpacing;
                _linePoints[1].position = pos += Vector3.left * _nodeSocketSpacing;
                break;

            default:
                _linePoints[0].position = pos;
                _linePoints[1].position = pos;
                break;
        }

        UpdateMidPoints();
    }
    public void SetSecondary(Node secondary)
    {
        _secondary = secondary;

        var pos = secondary.transform.position;

        switch (StartSocket)
        {
            case 0:
                _linePoints[4].position = pos += Vector3.up * _nodeSocketSpacing;
                _linePoints[3].position = pos += Vector3.up * _nodeSocketSpacing;
                break;

            case 1:
                _linePoints[4].position = pos += Vector3.right * _nodeSocketSpacing;
                _linePoints[3].position = pos += Vector3.right * _nodeSocketSpacing;
                break;

            case 2:
                _linePoints[4].position = pos += Vector3.down * _nodeSocketSpacing;
                _linePoints[3].position = pos += Vector3.down * _nodeSocketSpacing;
                break;

            case 3:
                _linePoints[4].position = pos += Vector3.left * _nodeSocketSpacing;
                _linePoints[3].position = pos += Vector3.left * _nodeSocketSpacing;
                break;

            default:
                _linePoints[3].position = pos;
                _linePoints[4].position = pos;
                break;
        }
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