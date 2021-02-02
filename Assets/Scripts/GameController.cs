using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour //script could do with a clean...
{
    [SerializeField] private Transform cam;
    [SerializeField] private Transform player;
    [SerializeField] private Transform backPad;

    [SerializeField] private GameObject copterPoop;

    private GameStatistics _currentGameStats;
    private DrawCaveWall _drawCaveWall;
    private Turret _turret;
    private Vector3 _playerStartPos, _backPadStartPos;

    public delegate void UpdateUITextDelegate(string newText);
    public event UpdateUITextDelegate OnScoreChanged;
    public event UpdateUITextDelegate OnPlayerDeath;
    public event UpdateUITextDelegate OnCountDown;

    private void Start()
    {
        player.gameObject.SetActive(false);
        _playerStartPos = player.position;
        _backPadStartPos = backPad.position;
        _currentGameStats = GetComponent<GameStatistics>();
        _drawCaveWall = GetComponent<DrawCaveWall>();
        _turret = player.GetComponentInChildren<Turret>();
    }
    private void OnGameStart()
    {
        player.gameObject.SetActive(true);
        player.position = _playerStartPos;
        cam.position = Vector3.zero;
        backPad.position = _backPadStartPos;

        InitialiseUI();
        StartCoroutine(KeepScore());
        StartCoroutine(FollowPlayer(cam, player));
        StartCoroutine(BackPadMovement(backPad, player));
        //StartCoroutine(ExhaustFlumes(player));
        StartCoroutine(BlockerCounter());
        _drawCaveWall.BeginDraw();
        StartCoroutine(_turret.Fire());
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void StartGameButton()
    {
        StartCoroutine(CountDownTimer());
    }

    private void InitialiseUI()
    {
        OnScoreChanged?.Invoke(newText: _currentGameStats.Score.ToString());
        OnCountDown?.Invoke(newText: "");
    }

    public IEnumerator KeepScore() //score increments while the player is alive
    {
        while (PlayerController._instance.gameObject.activeInHierarchy)
        {
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
        OnGameStart();
        yield return null;
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

    IEnumerator BlockerCounter()
    {
        while (PlayerController._instance.gameObject.activeInHierarchy)
        {
            float timer = Random.Range(2, 6);
            while(timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            RandomlySpawnBlocker();
        }
    }

    public void RandomlySpawnBlocker()
    {
        int rand = Random.Range(-30, 30);
        Vector3 position = new Vector3(player.transform.position.x + 200, rand, player.transform.position.z);
        GameObject blocker = PoolManager._instance.GetFromPool(1);
        blocker.transform.position = position;
        blocker.SetActive(true);
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("Main");
    }  

    IEnumerator Move(Transform start, Vector3 to, float dur)
    {
        float counter = 0;
        Vector3 startPos = start.position;
        while(counter < dur)
        {
            counter += Time.deltaTime;
            start.position = Vector3.Lerp(startPos, to, counter / dur);
            yield return null;
        }
    }

    public IEnumerator FollowPlayer(Transform toFollow, Transform player)
    {
        float distanceToPlayerX = toFollow.position.x - player.position.x;
        //float distanceTpPlayerY = toFollow.position.y - player.position.y;
        while (PlayerController._instance.gameObject.activeInHierarchy)
        {
            if (player.gameObject.activeInHierarchy)
            {
                float targetX = player.transform.position.x;
                //float targetY = player.transform.position.y;

                Vector3 newPos = toFollow.transform.position;
                newPos.x = targetX + distanceToPlayerX;
                //newPos.y = targetY + distanceTpPlayerY; - to follow in the y axis
                toFollow.position = newPos;
            }
            yield return null;
        }
    }   

    public IEnumerator ExhaustFlumes(Transform _player)
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
