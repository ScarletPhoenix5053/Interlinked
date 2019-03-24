using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    // Inspector
    [SerializeField] private bool _logInfo = false;
    [Header("Keys")]
    [SerializeField] private KeyCode _createLinkTool;
    [SerializeField] private KeyCode _upgradeLinkTool;
    [SerializeField] private KeyCode _upgradeNodeTool;

    // Events
    public event MouseClickEventHandler OnMouseClick;
    public event MouseHoverEventHandler OnMouseHover;

    // Input
    private const KeyCode _lmb = KeyCode.Mouse0;
    private InputTool _inputTool = InputTool.CreateLink;
    private InputTool _prevInputTool = InputTool.UpgradeLink;

    // Linking nodes
    private bool _linkingNodes;
    private Socket _prevClickedSocket;

    private NodeLink _previewLink;
    private Socket _previewLinkSocket;

    private void Awake()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(_lmb)) CreateLinkClick();
    }

    private void CreateLinkClick()
    {
        RaycastHit2D screenToPoint = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.MaxValue, LayerMask.GetMask("Socket"));
        if (screenToPoint.collider != null)
        {
            // Check if hit socket
            Socket currentSocket;
            if (currentSocket = screenToPoint.collider.GetComponent<Socket>())
            {
                // Exit early if in use
                if (currentSocket.InUse) return;

                // If already linking nodes
                if (_linkingNodes)
                {
                    // socket isn't connecting to own node
                    if (_prevClickedSocket.Parent == currentSocket.Parent)
                    {
                        if (_logInfo) Debug.Log("Can't create link for because both sockets are attatched to the same node.", currentSocket.Parent);

                        // reset
                        _linkingNodes = false;
                        return;
                    }

                    var cost = (int)Math.Truncate(Vector3.Distance(_prevClickedSocket.transform.position, currentSocket.transform.position) * LevelManager.Instance.ResourceDrainMultiplier);
                    if (LevelManager.Instance.UseResource(cost))
                    {
                        if (_logInfo) Debug.Log("Created link for " + cost + ". Remaining resource: " + LevelManager.Instance.Resource);

                        // spawn link
                        var link = Instantiate(Resources.Load<GameObject>("NodeLink").GetComponent<NodeLink>());

                        // connect to sockets
                        _prevClickedSocket.AttatchLinkAsPrimary(ref link);
                        currentSocket.AttatchLinkAsSecondary(ref link);
                        Debug.Assert(link.Secondary == currentSocket.Parent);

                    }
                    else
                    {
                        if (_logInfo) Debug.Log("Can't create link for " + cost + " because you only have " + LevelManager.Instance.Resource);
                    }
                    // reset
                    _linkingNodes = false;
                }
                else
                {
                    _prevClickedSocket = currentSocket;
                    _linkingNodes = true;

                    if (_logInfo) Debug.Log("Started connection from " + _prevClickedSocket);
                }
            }
        }
    }
    private void CreateLinkPreview()
    {
        var screenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        screenToWorldPoint.z = 0;
        RaycastHit2D screenToPointRay = Physics2D.Raycast(screenToWorldPoint, Vector2.zero, float.MaxValue, LayerMask.GetMask("Socket"));

        // If already linking nodes
        if (_linkingNodes)
        {
            // Create link preview to mouse pos
            _previewLink.GetComponent<LineRenderer>().enabled = true;
            _previewLinkSocket.transform.position = screenToWorldPoint;
            _previewLink.SetSecondary(_previewLinkSocket);
        }
        else
        {
            // Hide line preivew
            _previewLink.GetComponent<LineRenderer>().enabled = false;

            // Highlight a socket when hovering over it
            if (screenToPointRay.collider != null)
            {
                var currentSocket = screenToPointRay.collider.GetComponent<Socket>();
                currentSocket.Highlight();
            }
        }
    }
}
public enum InputTool
{
    CreateLink,
    UpgradeNode,
    UpgradeLink
}
public delegate void MouseHoverEventHandler();
public delegate void MouseClickEventHandler();