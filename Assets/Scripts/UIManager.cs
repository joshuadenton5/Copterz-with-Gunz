using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text restartText;
    [SerializeField] private Text countDownText;
    [SerializeField] private Text loadText;
    [SerializeField] private Text ammoText;

    [SerializeField] private Canvas loadCanvas;
    [SerializeField] private Canvas gameCanvas;

    private static UIManager _instnace;
    private bool toMain;

    [SerializeField] private Menu[] menus; //0 is main menu, 1 is pause menu

    private void Awake()
    {
        _instnace = this;
        restartText.text = "";
        scoreText.text = "";
        ammoText.text = "";
    }
    private void OnEnable()
    {
        gameController.OnScoreChanged += UpdateScore;
        gameController.OnPlayerDeath += OnDeath;
        gameController.OnCountDown += UpdateTimer;
        gameController.StartPauseWatch += CheckForPause;
        gameController.OnAmmoChange += UpdateAmmo;

    }
    private void OnDisable()
    {
        gameController.OnScoreChanged -= UpdateScore;
        gameController.OnPlayerDeath -= OnDeath;
        gameController.OnCountDown -= UpdateTimer;
        gameController.StartPauseWatch -= CheckForPause;
        gameController.OnAmmoChange -= UpdateAmmo;
    }

    public void UpdateAmmo(string _newText) 
    {
        ammoText.text ="Ammo:" + _newText;
    }
    public void UpdateTimer(string _newText)
    {
        countDownText.text = _newText;
    }
    public void UpdateScore(string newText)
    {
        scoreText.text = "Score:" + newText;
    }

    public void OnDeath(string newText)
    {
        restartText.text = newText;
        StartCoroutine(FlashText(restartText));
    }
    public void StartGame()
    {
        menus[0].PlayFromStart();
    }

    public void OnExit()
    {
        menus[0].Exit();
    }

    public void ResumeGame()
    {
        menus[1].Resume();
    }

    public void RestartGame()
    {
        menus[1].PlayFromStart();
    }
    public void ToMain()
    {
        gameController.OnRestart();
    }

    public void CheckForPause(string text)
    {
        StartCoroutine(PauseWatch());
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

    public IEnumerator PauseWatch()
    {
        while (PlayerController._instance.gameObject.activeInHierarchy)
        {
            bool _pause = Input.GetKeyDown(KeyCode.Escape);
            if (_pause)
            {
                menus[1].PauseGame();
            }
            yield return null;
        }
    }

    IEnumerator WaitForRestart()
    {
        while (!toMain)
        {
            toMain = Input.GetKeyDown(KeyCode.Return);
            yield return null;
        }
        //function is called several times due to multiple collisions. 
    }

    public IEnumerator FlashText(Text text) //need to update 
    {
        StartCoroutine(WaitForRestart());

        while (!toMain)
        {
            yield return new WaitForSeconds(.75f);
            if (toMain)
                break;
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);//setting alpha to zero
            yield return new WaitForSeconds(.75f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);//setting alpha to one
        }
        restartText.text = "";
        //could add loading coroutine here
        yield return Load();
        yield return gameController.OnRestart();
    }
    IEnumerator Load()
    {
        gameCanvas.gameObject.SetActive(false);
        loadCanvas.gameObject.SetActive(true);
        loadText.text = "Loading";
        int rand = Random.Range(3, 5);
        while (rand > 0)
        {
            rand -= 1;
            loadText.text += ".";
            yield return new WaitForSecondsRealtime(1);
        }
    }

    public static void FadeTextPopUp(Text text, float dur)
    {
         _instnace.StartCoroutine(_instnace.FadeTextOut(text, dur));
    }
}
