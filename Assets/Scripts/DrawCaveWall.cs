using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCaveWall : MonoBehaviour
{
    [SerializeField] private Transform startPositionTop;
    [SerializeField] private Transform startPositionBottom;
    private PlayerController playerController;
    [Range(0, 10)][SerializeField] private int caveLimits = 6;
    [SerializeField] private int initalLineNum;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        BeginDraw();
    }

    public void UpDifficulty() { caveLimits++; } //for difficulty changes

    public void BeginDraw()
    {
        StartCoroutine(DrawStraightWall(startPositionTop, initalLineNum));
        StartCoroutine(DrawStraightWall(startPositionBottom, initalLineNum));
    }

    private IEnumerator DrawStraightWall(Transform startPos, int amount)
    {
        Transform current = startPos;
        for (int i = 0; i < amount; i++)
        {
            GameObject pixel = PoolManager.SharedInstance.GetFromPool(0);
            Vector3 pos = new Vector3(current.position.x + pixel.transform.localScale.x, current.position.y, current.position.z);
            pixel.transform.position = pos;
            pixel.SetActive(true);
            current = pixel.transform;
            yield return null;
        }
        yield return DrawRandomWall(current);
    }

    public IEnumerator DrawRandomWall(Transform startPos)
    {
        float wait = 0;//need to create relationship with player speed....
        Vector3 start = startPos.position;
        Transform current = startPos;
        while (PlayerController.SharedInstance.gameObject.activeInHierarchy) //player is not dead 
        {
            GameObject pixel = PoolManager.SharedInstance.GetFromPool(0);//getting from pool 
            if (!(pixel is null)) 
            {
                int r = 0;
                float _scaleFactor = pixel.transform.localScale.x;
                wait = _scaleFactor / playerController.PlayerSpeed();
                if (current.position.y >= start.y + caveLimits)
                {
                    r = (int)-_scaleFactor;
                }
                else if(current.position.y <= start.y - caveLimits)
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
            }
            else
            {
                Debug.Log("None left in the pool!!");
                yield return new WaitForSeconds(1f); //waiting while the pool refills 
            }           
            yield return new WaitForSeconds(wait);
        }
    }
}
