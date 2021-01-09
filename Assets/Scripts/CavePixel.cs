using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavePixel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("End"))
        {
            if (gameObject.CompareTag("Cave"))
            {
                gameObject.SetActive(false);
            }            
        }
        else if (collision.CompareTag("Bullet"))
        {
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Block"))
        {
            //get parent GO
            transform.parent.gameObject.SetActive(false);
        }
    }
}
