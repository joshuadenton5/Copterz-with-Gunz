using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Transform player;
    [SerializeField] private Transform backPad;

    [SerializeField] private GameObject copterPoop, blocker;

    private GameStatistics _currentGameStats;

    public delegate void UpdateUITextDelegate(string newText);
    public event UpdateUITextDelegate OnScoreChanged;

    private void Start()
    {
        _currentGameStats = GetComponent<GameStatistics>();

        InitialiseUI();
        StartCoroutine(KeepScore());
        StartCoroutine(FollowPlayer(cam, player));
        StartCoroutine(BackPadMovement(backPad, player));
        //StartCoroutine(ExhaustFlumes(player));
        StartCoroutine(BlockerCounter());
    }

    private void InitialiseUI()
    {
        OnScoreChanged?.Invoke(newText: _currentGameStats.Score.ToString());
    }  

    public IEnumerator KeepScore() //score increments while the player is alive
    {
        while (PlayerController.SharedInstance.gameObject.activeInHierarchy)
        {
            UpdateScoreVal(.5f);
            yield return null;
        }
    }

    public void UpdateScoreVal(float val)
    {
        _currentGameStats.IncrementScore(val);
        OnScoreChanged?.Invoke(newText: Mathf.Round(_currentGameStats.Score).ToString());
    }

    IEnumerator BlockerCounter()
    {
        while (PlayerController.SharedInstance.gameObject.activeInHierarchy)
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
        int rand = Random.Range(-35, 35);
        Vector3 position = new Vector3(player.transform.position.x + 200, rand, player.transform.position.z);
        GameObject blocker = PoolManager.SharedInstance.GetFromPool(1);
        blocker.transform.position = position;
        blocker.SetActive(true);
    }

    public IEnumerator FollowPlayer(Transform toFollow, Transform player)
    {
        float distanceToPlayerX = toFollow.position.x - player.position.x;
        float distanceTpPlayerY = toFollow.position.y - player.position.y;
        while (PlayerController.SharedInstance.gameObject.activeInHierarchy)
        {
            if (player.gameObject.activeInHierarchy)
            {
                float targetX = player.transform.position.x;
                float targetY = player.transform.position.y;

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
        while (PlayerController.SharedInstance.gameObject.activeInHierarchy)
        {
            Instantiate(copterPoop, point.transform.position, copterPoop.transform.rotation);
            yield return null;
        }
    }

    IEnumerator BackPadMovement(Transform backPad, Transform player)
    {
        Vector3 difference = backPad.position - player.position;
        float followSharpness = 0.1f;
        while (PlayerController.SharedInstance.gameObject.activeInHierarchy)
        {
            Vector3 targetPos = player.position + difference;
            targetPos.y = backPad.position.y;
            backPad.position += (targetPos - backPad.position) * followSharpness;
            yield return null;
        }
    }
}
