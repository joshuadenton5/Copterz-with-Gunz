using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] protected GameObject _panel;
    protected GameController _gameController;

    protected virtual void Start()
    {
        _gameController = FindObjectOfType<GameController>();
    }

    public virtual void PlayFromStart()
    {
        _gameController.StartGameButton();
        _panel.SetActive(false);
    }

    public virtual void PauseGame()
    {
        _gameController.GameStopped = true;
    }   
    public void Exit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _panel.SetActive(false);
        _gameController.GameStopped = false;
    }
    public void Reload()
    {
        _gameController.OnRestart();
    }
}
