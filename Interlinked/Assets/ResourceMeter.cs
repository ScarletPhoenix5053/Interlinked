using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ResourceMeter : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }
    void FixedUpdate()
    {
        text.text = "Resource: " + LevelManager.Instance.Resource;
    }
}
