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
    private List<SocketedNode> _nodes = new List<SocketedNode>();

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
                SocketedNode node;
                if (node = child.GetComponent<SocketedNode>())
                {
                    _nodes.Add(node);
                }
            }
        }

        // Spawn pops
        _nodes[0].SpawnPops(5, _defaultRoutine);
        _nodes[0].ContainedPops[1].LogInfo = true;

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