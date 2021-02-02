using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    private GameController _gameController;

    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
    }

    public void StartGame()
    {
        _gameController.StartGameButton();
        _panel.SetActive(false);
    }

    public void Exit()
    {
        _gameController.OnExit();
    }
}
