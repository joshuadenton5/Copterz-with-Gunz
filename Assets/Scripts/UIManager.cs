using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Text scoreText;
    private static UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        gameController.OnScoreChanged += UpdateScore;
    }
    private void OnDisable()
    {
        gameController.OnScoreChanged -= UpdateScore;
    }

    public void UpdateScore(string newText)
    {
        scoreText.text = "Score:"+ newText;
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

    public static void FadeTextPopUp(Text text, float dur)
    {
         instance.StartCoroutine(instance.FadeTextOut(text, dur));
    }
}
