using UnityEngine;
using System.Collections;

public class Socket : MonoBehaviour
{
    [SerializeField] private NodeLink _attatchedLink;
    [SerializeField] private float _highlightDuration;

    private SpriteRenderer _spriteRenderer;
    private Color _colourHighlight = Color.cyan;
    private Color _colourNormal = Color.black;
    private IEnumerator _currentHighlightRoutine = null;

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
    private void Update()
    {
        
    }

    public bool AttatchLinkAsPrimary(ref NodeLink nodeLink)
    {
        if (InUse) return false;

        AttatchedLink = nodeLink;
        //Debug.Log("Attatching " + nodeLink + " to " + name + " as primary");
        Debug.Assert(AttatchedLink != null);
        nodeLink.SetPrimary(this);
        _spriteRenderer.sprite = Resources.Load<Sprite>("NodeSocket Outgoing");

        return true;
    }
    public bool AttatchLinkAsSecondary(ref NodeLink nodeLink)
    {
        if (InUse) return false;

        AttatchedLink = nodeLink;
        //Debug.Log("Attatching " + nodeLink + " to " + name + " as secondary");
        Debug.Assert(AttatchedLink != null);
        nodeLink.SetSecondary(this);
        _spriteRenderer.sprite = Resources.Load<Sprite>("NodeSocket Incoming");

        return true;
    }
    public void Highlight()
    {
        Debug.Log("Highlight caled");
        if (_currentHighlightRoutine != null) StopCoroutine(_currentHighlightRoutine);
        _currentHighlightRoutine = HighLightRoutine();
        StartCoroutine(_currentHighlightRoutine);
    }
    private IEnumerator HighLightRoutine()
    {
        _spriteRenderer.color = _colourHighlight;

        var time = 0f;
        while (time < _highlightDuration)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _spriteRenderer.color = _colourNormal;
    }
}
