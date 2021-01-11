using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Text scoreText;

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

}
