using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : Menu
{
    protected override void Start()
    {
        base.Start();
        _panel.SetActive(false);
    }
    public override void PauseGame()
    {
        base.PauseGame();
        _panel.SetActive(true);
        Time.timeScale = 0;
    }
    public override void PlayFromStart()
    {
        base.PlayFromStart();
        Time.timeScale = 1;
        Debug.Log("Test");
    }
}
