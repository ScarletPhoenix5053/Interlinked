using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    // Inspector vars
    [SerializeField] private Transform _popsContainter;
    [SerializeField] private Transform _nodesContainter;
    [SerializeField] private Routine _defaultRoutine;

    // Private fields
    private List<Node> _nodes = new List<Node>();

    // Properties
    public static LevelManager Instance { get; private set; }
    public Transform PopsContainer { get { return _popsContainter; } }

    // Events
    public static event TrafficOverloadEventHandler OnTrafficOverload;
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);

        // Assign initial nodes
        if (_nodesContainter != null)
        {
            foreach (Transform child in _nodesContainter)
            {
                Node node;
                if (node = child.GetComponent<Node>())
                {
                    _nodes.Add(node);
                }
            }
        }

        // Spawn nodes
        _nodes[0].SpawnPops(1, _defaultRoutine);

        // Link nodes
        _nodes[0].ConnectTo(_nodes[1]);
        Debug.Assert(_nodes[0].Connections[0].StartSocket == 0);
        Debug.Assert(_nodes[0].Connections[0].EndSocket == 0);
        Debug.Assert(_nodes[0].NCI == 1);
        Debug.Assert(_nodes[1].NCI == 1);
        _nodes[1].ConnectTo(_nodes[0]);
        Debug.Assert(_nodes[1].Connections[0].StartSocket == 1);
        Debug.Assert(_nodes[1].Connections[0].EndSocket == 1);
        Debug.Assert(_nodes[0].NCI == 2);
        Debug.Assert(_nodes[1].NCI == 2);

        // Config events
        OnTrafficOverload += RestartGame;
    }

    public static void TiggerTraficOverload()
    {
        OnTrafficOverload?.Invoke();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
public delegate void TrafficOverloadEventHandler();