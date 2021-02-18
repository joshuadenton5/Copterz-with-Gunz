using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour //script could do with a clean...
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform backPad;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject copterPoop;

    private GameStatistics _currentGameStats;
    private DrawCaveWall _drawCaveWall;
    private Vector3 _playerStartPos, _backPadStartPos;

    private GameObject player;
    public bool GameStopped { get; set; }

    public delegate void UpdateUITextDelegate(string newText);
    public event UpdateUITextDelegate OnScoreChanged;
    public event UpdateUITextDelegate OnPlayerDeath;
    public event UpdateUITextDelegate OnCountDown;
    public event UpdateUITextDelegate StartPauseWatch; //probably should create seperate delegate
    public event UpdateUITextDelegate OnAmmoChange;

    private void Start()
    {
        _playerStartPos = new Vector3(-88, 0, 125);
        _backPadStartPos = backPad.position;
        _currentGameStats = GetComponent<GameStatistics>();
        _drawCaveWall = GetComponent<DrawCaveWall>();
        GameObject _player = Instantiate(playerPrefab, _playerStartPos, playerPrefab.transform.rotation);
        player = _player;
        player.SetActive(false);
    }
    private void OnGameStart()
    {
        player.SetActive(true);
        _camera.position = Vector3.zero;
        backPad.position = _backPadStartPos;
        InitialiseUI();
        StartCoroutine(KeepScore());
        StartCoroutine(FollowPlayer(_camera, player.transform));
        StartCoroutine(BackPadMovement(backPad, player.transform));
        //StartCoroutine(ExhaustFlumes(player));
        //StartCoroutine(BlockerCounter(_player));
        CavePixel.pixelHit = false; //prevents multiple death events firing if player collided with mulitple avoidables
    }  

    public void StartGameButton()
    {
        StartCoroutine(CountDownTimer());
    }

    private void InitialiseUI()
    {
        OnScoreChanged?.Invoke(newText: _currentGameStats.Score.ToString());
        OnCountDown?.Invoke(newText: "");
        StartPauseWatch?.Invoke("");
        OnAmmoChange?.Invoke(newText:_currentGameStats.Ammo.ToString());
    }

    public IEnumerator KeepScore() //score increments while the player is alive
    {
        while (PlayerController._instance.gameObject.activeInHierarchy)
        {
            while (GameStopped)
                yield return null;
            UpdateScoreVal(.5f);
            yield return null;
        }
    }
    public IEnumerator CountDownTimer()
    {
        float timer = 3;
        while(timer > 0)
        {
            OnCountDown?.Invoke(newText: Mathf.Round(timer).ToString());
            timer -= Time.deltaTime;
            yield return null;
        }
        OnCountDown?.Invoke(newText: "");
        _drawCaveWall.BeginDraw();
        OnGameStart();
    }

    public int GetCurrentAmmo()
    {
        return _currentGameStats.Ammo;
    }

    public void UpdateAmmoAmount(int _amount)
    {
        _currentGameStats.DecrementAmmo(_amount);
        OnAmmoChange?.Invoke(newText: _currentGameStats.Ammo.ToString());
    }

    public void UpdateScoreVal(float val)
    {
        _currentGameStats.IncrementScore(val);
        OnScoreChanged?.Invoke(newText: Mathf.Round(_currentGameStats.Score).ToString());
    }

    public void PlayerIsDead()
    {
        _currentGameStats.IncrementDeathVal();
        OnPlayerDeath?.Invoke(newText: "U r Ded - Return::Main");
    }

    IEnumerator BlockerCounter(GameObject _player)
    {
        while (PlayerController._instance.gameObject.activeInHierarchy)
        {
            float timer = Random.Range(2, 6);
            while(timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            RandomlySpawnBlocker(_player);
        }
    }

    public void RandomlySpawnBlocker(GameObject _player)//need to update 
    {
        int rand = Random.Range(-30, 30);
        Vector3 position = new Vector3(_player.transform.position.x + 200, rand, _player.transform.position.z);
        GameObject blocker = PoolManager._instance.GetFromPool(1);
        blocker.transform.position = position;
        blocker.SetActive(true);
    }

    public IEnumerator OnRestart()
    {
        AsyncOperation loadMain = SceneManager.LoadSceneAsync("Main");
        while (!loadMain.isDone)
        {
            yield return null;
        }
    }    

    public IEnumerator FollowPlayer(Transform toFollow, Transform player)
    {
        float distanceToPlayerX = toFollow.position.x - player.position.x;
        while (PlayerController._instance.gameObject.activeInHierarchy)
        {
            if (player.gameObject.activeInHierarchy)
            {
                float targetX = player.transform.position.x;

                Vector3 newPos = toFollow.transform.position;
                newPos.x = targetX + distanceToPlayerX;
                toFollow.position = newPos;
            }
            yield return null;
        }
    }   

    public IEnumerator ExhaustFlumes(Transform _player) //want a pixelated effect
    {
        Transform point = _player.Find("PoopPoint");
        while (PlayerController._instance.gameObject.activeInHierarchy)
        {
            Instantiate(copterPoop, point.transform.position, copterPoop.transform.rotation);
            yield return null;
        }
    }

    IEnumerator BackPadMovement(Transform backPad, Transform player)
    {
        Vector3 difference = backPad.position - player.position;
        float followSharpness = 0.1f;
        while (PlayerController._instance.gameObject.activeInHierarchy)
        {
            Vector3 targetPos = player.position + difference;
            targetPos.y = backPad.position.y;
            backPad.position += (targetPos - backPad.position) * followSharpness;
            yield return null;
        }
    }
}
