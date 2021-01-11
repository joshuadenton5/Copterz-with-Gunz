using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatistics : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    public float Score { get; set; }

    void Start()
    {
        Score = 0;
    }

    public void IncrementScore(float val)
    {
        Score += val;
    }
}
