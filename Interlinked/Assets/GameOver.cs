using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private Canvas _gameOverCanvas;

    private void Awake()
    {
        _gameOverCanvas = GetComponent<Canvas>();
    }

    public void SetGameOver()
    {
        _gameOverCanvas.enabled = true;
    }
    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
