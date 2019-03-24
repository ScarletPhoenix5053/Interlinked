using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shaker : MonoBehaviour
{
    private static Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    public static void ShakeLight()
    {
        cam.DOShakePosition(0.1f, 1, 1);
    }
    public static void ShakeHard()
    {
        Debug.Log("Called");
        cam.DOShakePosition(0.8f, 3, 10);
    }
}
