using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameStart : MonoBehaviour
{
    [SerializeField] private Canvas GameStartCanvas;
    [SerializeField] private Canvas HelpScreenCanvas;


    private void Start()
    {
        ShowMenu();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMenu();
        }
    }

    public void StartGame()
    {
        GameStartCanvas.enabled = false;
        HelpScreenCanvas.enabled = false;

        Time.timeScale = 1;
    }
    public void ShowMenu()
    {
        GameStartCanvas.enabled = true;
        HelpScreenCanvas.enabled = false;

        Time.timeScale = 0;
    }
    public void ShowHelp()
    {
        GameStartCanvas.enabled = false;
        HelpScreenCanvas.enabled = true;

        Time.timeScale = 0;
    }
}
