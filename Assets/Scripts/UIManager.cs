using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text restartText;

    private static UIManager _instnace;
    private bool restart;
    private void Awake()
    {
        _instnace = this;
        restartText.text = "";
    }
    private void OnEnable()
    {
        gameController.OnScoreChanged += UpdateScore;
        gameController.OnPlayerDeath += OnDeath;
    }
    private void OnDisable()
    {
        gameController.OnScoreChanged -= UpdateScore;
        gameController.OnPlayerDeath -= OnDeath;
    }

    public void UpdateScore(string newText)
    {
        scoreText.text = "Score:"+ newText;
    }

    public void OnDeath(string newText)
    {
        restartText.text = newText;
        StartCoroutine(FlashText(restartText));
    }

    public IEnumerator FadeTextOut(Text text, float dur)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime / dur);
            yield return null;
        }
    }

    IEnumerator WaitForRestart()
    {
        while (!restart)
        {
            restart = Input.GetKeyDown(KeyCode.R);
            yield return null;
        }
        //Debug.Log("oi cunt");//function is called several times due to multiple collisions. 
    }

    public IEnumerator FlashText(Text text) //need to update 
    {
        StartCoroutine(WaitForRestart());

        while (!restart)
        {
            yield return new WaitForSeconds(.75f);
            if (restart)
                break;
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);//setting alpha to zero
            yield return new WaitForSeconds(.75f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);//setting alpha to one
        }
        restartText.text = "";
        gameController.OnRestart();
    }

    public static void FadeTextPopUp(Text text, float dur)
    {
         _instnace.StartCoroutine(_instnace.FadeTextOut(text, dur));
    }
}
