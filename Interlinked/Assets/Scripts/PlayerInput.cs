using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private bool _logInfo = false;

    private const KeyCode _lmb = KeyCode.Mouse0;
    private bool _linkingNodes;
    private Node _prevClickedNode;

    private void Update()
    {
        RaycastHit2D screenToPoint = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        // On left click
        if (Input.GetKeyDown(_lmb))
        {
            if (screenToPoint.collider != null)
            {
                // Check if hit node
                Node currentNode;
                if (currentNode = screenToPoint.collider.GetComponent<Node>())
                {
                    // If already linking nodes
                    if (_linkingNodes)
                    {
                        // Finish connection
                        if (_prevClickedNode.ConnectTo(currentNode) && _logInfo)
                            Debug.Log("Completed connection from " + _prevClickedNode + " to " + currentNode);
                        else
                            Debug.Log("Failed connection from " + _prevClickedNode + " to " + currentNode);
                        _prevClickedNode = null;
                        _linkingNodes = false;
                    }
                    else
                    {
                        // Start connection
                        _prevClickedNode = currentNode;
                        _linkingNodes = true;

                        if (_logInfo) Debug.Log("Started connection from " + _prevClickedNode);
                    }
                }
            }
        }
    }
}
