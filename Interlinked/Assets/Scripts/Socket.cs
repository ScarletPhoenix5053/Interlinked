using UnityEngine;
using System.Collections;

public class Socket : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public SocketedNode Parent { get; set; }
    public Vector3 OffsetDir { get; set; }
    public NodeLink AttatchedLink { get; private set; }
    public bool InUse { get { return AttatchedLink != null; } }

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        OffsetDir = Vector3.up;
    }

    public void AttatchLinkAsPrimary(NodeLink nodeLink)
    {
        AttatchedLink = nodeLink;
        nodeLink.SetPrimary(this);
        _spriteRenderer.sprite = Resources.Load<Sprite>("NodeSocket Outgoing");
    }
    public void AttatchLinkAsSecondary(NodeLink nodeLink)
    {
        AttatchedLink = nodeLink;
        nodeLink.SetSecondary(this);
        _spriteRenderer.sprite = Resources.Load<Sprite>("NodeSocket Incoming");
    }
}
