using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DrawCaveWall : MonoBehaviour
{
    [SerializeField] private Transform startPositionTop;
    [SerializeField] private Transform startPositionBottom;
    [Range(0, 40)][SerializeField] private int caveLimits = 6;
    [SerializeField] private int initalLineNum;
    [SerializeField] private int pixelCounter;

    [SerializeField] private GameController _gameController;
    [SerializeField] private GameObject pixelSpawnTrigger;
    [SerializeField] int _amount;

    public void UpDifficulty() { caveLimits++; } //for difficulty changes

    bool startClock;
    float timeLag;

    public void BeginDraw()
    {
        //StartCoroutine(DrawStraightWall(startPositionTop, initalLineNum));
        //StartCoroutine(DrawStraightWall(startPositionBottom, initalLineNum));
        StartCoroutine(Test(_amount,startPositionTop));
        StartCoroutine(Test(_amount, startPositionBottom));
    }
    public IEnumerator DrawRandomWall(Transform startPos)
    {
        float _wait = 0;//need to create relationship with player speed....
        Vector3 start = startPos.position;
        Transform current = startPos;
        while (!PlayerController._instance.isDead && !_gameController.GameStopped) //player is not dead 
        {
            GameObject pixel = PoolManager._instance.GetFromPool(0);//getting from pool 
            startClock = true; //slight time lag... meaning the spawn rate is slightly behind the player
            if (!(pixel is null))
            {
                float _scaleFactor = pixel.transform.localScale.x;
                _wait = _scaleFactor / PlayerController._speed;//need to figure out
                int r;
                if (current.position.y >= start.y + caveLimits)
                {
                    r = (int)-_scaleFactor;
                }
                else if (current.position.y <= start.y - caveLimits)
                {
                    r = (int)_scaleFactor;
                }
                else
                {
                    r = Random.Range(-1, 2) * (int)_scaleFactor;
                }
                Vector3 pos = new Vector3(current.position.x + _scaleFactor, current.position.y + r, current.position.z);
                current = pixel.transform;
                pixel.transform.position = pos;
                pixel.SetActive(true);
                pixelCounter++;
            }
            else
            {
                Debug.LogError("None left in the pool!!");
                yield return new WaitForSecondsRealtime(1f); //waiting while the pool refills 
            }
            yield return new WaitForSeconds(_wait);
        }
    }

    private IEnumerator DrawStraightWall(Transform startPos, int amount)
    {
        Transform current = startPos;
        for (int i = 0; i < amount; i++)
        {
            GameObject pixel = PoolManager._instance.GetFromPool(0);
            Vector3 pos = new Vector3(current.position.x + pixel.transform.localScale.x, current.position.y, current.position.z);
            pixel.transform.position = pos;
            pixel.SetActive(true);
            current = pixel.transform;
            yield return null;
        }
        yield return DrawRandomWall(current);
    }

    public IEnumerator Test(int amount, Transform _startPos)
    {
        Transform current = _startPos;
        Vector3 start = _startPos.position;
        float _wait = 0;
        float _scaleFactor = 0;
        for (int i = 0; i < amount; i++)
        {
            GameObject pixel = PoolManager._instance.GetFromPool(0);//getting from pool 
            _scaleFactor = pixel.transform.localScale.x;
            if (!(pixel is null))
            {
                _wait = 0.009f;
                int r;
                if (current.position.y >= start.y + caveLimits)
                {
                    r = (int)-_scaleFactor;
                }
                else if (current.position.y <= start.y - caveLimits)
                {
                    r = (int)_scaleFactor;
                }
                else
                {
                    r = Random.Range(-1, 2) * (int)_scaleFactor;
                }
                Vector3 pixelSpawnPosition = new Vector3(current.position.x + _scaleFactor, current.position.y + r, current.position.z);
                current = pixel.transform;
                pixel.transform.position = pixelSpawnPosition;
                pixel.SetActive(true);
                pixelCounter++;
            }
            else
            {
                Debug.LogError("None left in the pool!!");
                yield return new WaitForSecondsRealtime(1f); //waiting while the pool refills 
            }
            yield return new WaitForSecondsRealtime(_wait);
        }
        float distX = amount * _scaleFactor;
        Vector2 pos = new Vector2(_startPos.position.x + distX, 0);
        GameObject pixelSpawner = Instantiate(pixelSpawnTrigger, pos, pixelSpawnTrigger.transform.rotation);
    }   
}
