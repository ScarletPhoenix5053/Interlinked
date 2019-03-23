using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private bool _logInfo = false;

    private const KeyCode _lmb = KeyCode.Mouse0;
    private bool _linkingNodes;
    private Socket _prevClickedSocket;

    private void Update()
    {
        RaycastHit2D screenToPoint = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.MaxValue, LayerMask.GetMask("Socket"));

        // On left click
        if (Input.GetKeyDown(_lmb))
        {
            if (screenToPoint.collider != null)
            {
                // Check if hit socket
                Socket currentSocket;
                if (currentSocket = screenToPoint.collider.GetComponent<Socket>())
                {
                    // If already linking nodes
                    if (_linkingNodes)
                    {
                        // spawn link
                        var link = Instantiate(Resources.Load<GameObject>("NodeLink").GetComponent<NodeLink>());

                        // connect to sockets
                        _prevClickedSocket.AttatchLinkAsPrimary(ref link);
                        currentSocket.AttatchLinkAsSecondary(ref link);
                        Debug.Assert(link.Secondary == currentSocket.Parent);

                        // reset
                        _linkingNodes = false;
                    }
                    else
                    {
                        // Start connection
                        _prevClickedSocket = currentSocket;
                        _linkingNodes = true;

                        if (_logInfo) Debug.Log("Started connection from " + _prevClickedSocket);
                    }
                }
            }
        }
    }
}
