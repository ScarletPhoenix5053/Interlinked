using UnityEngine;
using System.Collections;

public class Socket : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private NodeLink _attatchedLink;

    public SocketedNode Parent { get; set; }
    public Vector3 OffsetDir { get; set; }
    public NodeLink AttatchedLink
    {
        get
        {
            //Debug.Log(name + "'s attatched link was accessed", this);
            return _attatchedLink;
        }
        private set
        {
            //Debug.Log(name + "'s attatched link was modified: is now " + value, value);
            _attatchedLink = value;
        }
    }
    public bool InUse { get { return AttatchedLink != null; } }

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        OffsetDir = Vector3.up;
    }

    public void AttatchLinkAsPrimary(ref NodeLink nodeLink)
    {
        AttatchedLink = nodeLink;
        //Debug.Log("Attatching " + nodeLink + " to " + name + " as primary");
        Debug.Assert(AttatchedLink != null);
        nodeLink.SetPrimary(this);
        _spriteRenderer.sprite = Resources.Load<Sprite>("NodeSocket Outgoing");
    }
    public void AttatchLinkAsSecondary(ref NodeLink nodeLink)
    {
        AttatchedLink = nodeLink;
        //Debug.Log("Attatching " + nodeLink + " to " + name + " as secondary");
        Debug.Assert(AttatchedLink != null);
        nodeLink.SetSecondary(this);
        _spriteRenderer.sprite = Resources.Load<Sprite>("NodeSocket Incoming");
    }
}
