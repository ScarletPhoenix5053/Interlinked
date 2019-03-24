using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PopCounter : MonoBehaviour
{
    private SocketedNode _node;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _node = GetComponentInParent<SocketedNode>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        // Update display based on count of pops in node
        if (_node.ContainedPops.Count == 0)
        {
            _spriteRenderer.enabled = false;
        }
        else
        {
            _spriteRenderer.enabled = true;
            var spriteIndex = Mathf.Clamp(_node.ContainedPops.Count, 1, 8);
            _spriteRenderer.sprite = Resources.Load<Sprite>("Count " + spriteIndex);
        }
    }
}
