using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Transform cam, player;
    [SerializeField] private GameObject copterPoop, blocker;
    [SerializeField] private Transform backPad;
    public static float score;

    private void Awake()
    {
        score = 0;
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(FollowPlayer(cam, player));
        StartCoroutine(BackPadMovement(backPad, player));
        StartCoroutine(KeepScore());
        //StartCoroutine(ExhaustFlumes(player));
        StartCoroutine(BlockerCounter());
    } 

    public IEnumerator KeepScore()
    {
        while (!PlayerController.dead)
        {
            score += .001f;
            yield return null;
        }
    }

    public IEnumerator ExhaustFlumes(Transform _player)
    {
        Transform point = _player.Find("PoopPoint");
        while (!PlayerController.dead)
        {
            Instantiate(copterPoop, point.transform.position, copterPoop.transform.rotation);
            yield return null;
        }
    }   

    IEnumerator BackPadMovement(Transform backPad, Transform player)
    {
        Vector3 difference = backPad.position - player.position;
        float followSharpness = 0.1f;
        while (!PlayerController.dead)
        {
            Vector3 targetPos = player.position + difference;
            targetPos.y = backPad.position.y;
            backPad.position += (targetPos - backPad.position) * followSharpness;
            yield return null;
        }
    }

    IEnumerator BlockerCounter()
    {
        while (!PlayerController.dead)
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
        Vector3 position = new Vector3(player.transform.position.x + 200, rand, 125);
        GameObject blocker = PoolManager.SharedInstance.GetFromPool(1);
        blocker.transform.position = position;
        blocker.SetActive(true);
    }

    public IEnumerator FollowPlayer(Transform toFollow, Transform player)
    {
        float distanceToPlayerX = toFollow.position.x - player.position.x;
        float distanceTpPlayerY = toFollow.position.y - player.position.y;
        while (!PlayerController.dead)
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
}
