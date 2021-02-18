using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatistics : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    public float Score { get; set; }
    public float TimesKilled { get; set; }
    public int Ammo { get; set; }
    [SerializeField] private int _ammo;


void Start()
    {
        TimesKilled = 0;
        Score = 0;
        Ammo = _ammo;
    }
    public void DecrementAmmo(int val)
    {
        Ammo -= val;
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
