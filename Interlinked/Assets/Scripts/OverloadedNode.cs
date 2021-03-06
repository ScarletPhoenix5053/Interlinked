﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class OverloadedNode : MonoBehaviour
{
    // Inspector Vars
    /// <summary>
    /// Maximum time a congestion area can exist in seconds
    /// </summary>
    [SerializeField] private float _timeMax = 5f;
    /// <summary>
    /// Delay between time display filling up in game, and game over being triggered.
    /// </summary>
    [SerializeField] private float _timeOverload = 1f;
    /// <summary>
    /// Delay between congestion instance spawning, and the timer starting.
    /// </summary>
    [SerializeField] private float _timeDelay = 1f;

    [SerializeField] private GameObject EXPLOSION;

    // Private Vars
    private SpriteRenderer _spriteRenderer;
    private Sprite[] _timerSprites;
    private SocketedNode _parent;

    // Properties
    public float Timer { get; private set; }

    #region Unity Messages
    private void Awake()
    {
        // Initialize
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer == null) Debug.LogError("Please place a sprite renderer in a child of " + name, this);

        // Load timer sprites
        _timerSprites = new Sprite[13];
        for (int i = 0; i < _timerSprites.Length; i++)
        {
            _timerSprites[i] = Resources.Load<Sprite>("TrafficBlock An " + (i + 1));
        }

        // apply delay to timer
        Timer -= _timeDelay;
    }
    private void FixedUpdate()
    {
        // Incriment timer
        Timer += Time.fixedDeltaTime;

        // Update display
        if (_spriteRenderer != null && Timer > 0 && Timer < _timeMax)
        {
            // get angle from time
            var progress = Timer / _timeMax;
            var spriteIndex = Mathf.RoundToInt(Mathf.Clamp(progress * 13, 0, 12));

            // assign sprite by angle
            _spriteRenderer.sprite = _timerSprites[spriteIndex];
        }

        // Trigger game end if time is up
        if (Timer > _timeMax + _timeOverload)
        {
            Shaker.ShakeHard();
            if (EXPLOSION != null) Instantiate(EXPLOSION, transform.position, Quaternion.identity);
            LevelManager.TiggerTraficOverload();
            RemoveOverloadEffect();
        }
    }
    #endregion

    public static OverloadedNode SpawnCongestionZone(SocketedNode parent)
    {
        return Instantiate(Resources.Load<GameObject>("TrafficBlock"), parent.transform.position, Quaternion.identity).GetComponent<OverloadedNode>();
    }

    public void RemoveOverloadEffect()
    {
        Destroy(gameObject);
    }
}