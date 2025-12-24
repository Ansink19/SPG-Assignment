using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] private GameObject LoadingCanvasPanel;
    [SerializeField] private GameObject MenuCanvasPanel;
    [SerializeField] private GameObject GameCanvasPanel;

    [SerializeField] private Button PlayButton;

    [SerializeField] private GameObject Timer;
    [SerializeField] private Image TimerFill;

    [SerializeField] private RevelToken AIRevealToken;
    [SerializeField] private RevelToken PlayerRevealToken;

    [SerializeField] private GameObject Tost;
    [SerializeField] private TextMeshProUGUI ToastText;

    [SerializeField] private GameObject HighScore;
    [SerializeField] private TextMeshProUGUI HighScoreText;

    [SerializeField] private TextMeshProUGUI AIScoreText;
    [SerializeField] private TextMeshProUGUI PlayerScoreText;

    private World World;

    void Awake()
    {
        GameEventHandler.OnWorldInit += OnWorldInit;
        GameEventHandler.OnGameStateChange += OnGameStateChanged;
        LoadingCanvasPanel.SetActive(true);
    }

    public void OnWorldInit(World world)
    {
        World = world;
        PlayButton.onClick.AddListener(OnPlayButtonClicked);
    }

    public void OnPlayButtonClicked()
    {
        World.GameStart();
    }

    void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.Menu)
        {
            TurnOffAllPanels();
            MenuCanvasPanel.SetActive(true);
            SetHighScore();
        }
        else if (gameState == GameState.Play)
        {
            MenuCanvasPanel.SetActive(true);
            GameCanvasPanel.SetActive(true);
            StartCoroutine(PlayHandRoutine());
        }
        else if(gameState == GameState.Reveal)
        {
            StartCoroutine(RevealHandRoutine());
        }
    }

    private void SetHighScore()
    {
        if(World.PlayerHighScore != 0)
        {
            HighScore.SetActive(true);
            HighScoreText.text = $"{World.PlayerHighScore}";
        }
    }

    public void TurnOffAllPanels()
    {
        LoadingCanvasPanel.SetActive(false);
        MenuCanvasPanel.SetActive(false);
        GameCanvasPanel.SetActive(false);
    }

    private IEnumerator PlayHandRoutine()
    {
        yield return ShowToast($"Round Start!");

        Timer.SetActive(true);

        float timer = 0;
        float totalTime = World.PlayHandTime;
        TimerFill.fillAmount = 1;
        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            TimerFill.fillAmount = timer / totalTime;
            yield return null;
        }
        TimerFill.fillAmount = 0;
        yield return new WaitForSeconds(1f);

        World.UpdateGameState(GameState.Reveal);
    }

    private IEnumerator RevealHandRoutine()
    {
        Timer.SetActive(false);
        AIRevealToken.Init(World.AIElement, World.ElementMap.ElementIcons);
        PlayerRevealToken.Init(World.PlayerElement, World.ElementMap.ElementIcons);
        yield return new WaitForSeconds(1f);

        var winner = World.RoundWinner;
        yield return ShowToast($"{winner} Wins!!");

        AIScoreText.text = $"{World.AIScore}";
        PlayerScoreText.text = $"{World.PlayerScore}";
        yield return new WaitForSeconds(1f);
        World.UpdateGameState(GameState.PostGame);
    }

    private IEnumerator ShowToast(string text, float time = 1)
    {
        Tost.SetActive(true);
        ToastText.text = text;
        yield return new WaitForSeconds(time);
        Tost.SetActive(false);
    }

}
