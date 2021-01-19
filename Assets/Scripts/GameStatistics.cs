using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatistics : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    public float Score { get; set; }
    public float TimesKilled { get; set; }

    void Start()
    {
        TimesKilled = 0;
        Score = 0;
    }

    public void IncrementScore(float val)
    {
        Score += val;
    }

    public void IncrementDeathVal()
    {
        TimesKilled++;
    }
}
