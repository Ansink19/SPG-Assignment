using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject LoadingCanvasPanel;
    [SerializeField] private GameObject MenuCanvasPanel;
    [SerializeField] private GameObject GameCanvasPanel;

    [Space]
    [Header("Menu")]
    [SerializeField] private Button PlayButton;
    [SerializeField] private GameObject HighScore;
    [SerializeField] private TextMeshProUGUI HighScoreText;

    [Space]
    [Header("Game")]
    [SerializeField] private GameObject Timer;
    [SerializeField] private Image TimerFill;

    [Space]
    [SerializeField] private RevealToken AIRevealToken;
    [SerializeField] private RevealToken PlayerRevealToken;

    [Space]
    [SerializeField] private GameObject Tost;
    [SerializeField] private TextMeshProUGUI ToastText;

    [Space]
    [SerializeField] private TextMeshProUGUI AIScoreText;
    [SerializeField] private TextMeshProUGUI PlayerScoreText;

    private World World;

    private void Awake()
    {
        GameEventHandler.OnWorldInit += OnWorldInit;
        GameEventHandler.OnGameStateChange += OnGameStateChanged;
        LoadingCanvasPanel.SetActive(true);
        Tost.SetActive(false);
        Timer.SetActive(false);
    }

    private void OnWorldInit(World world)
    {
        World = world;
        PlayButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        World.GameStart();
    }

    void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.Menu)
        {
            TurnOffAllPanels();
            SetHighScore();
            MenuCanvasPanel.SetActive(true);
        }
        else if (gameState == GameState.Play)
        {
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
            return;
        }
        HighScore.SetActive(false);
    }

    private void TurnOffAllPanels()
    {
        LoadingCanvasPanel.SetActive(false);
        MenuCanvasPanel.SetActive(false);
        GameCanvasPanel.SetActive(false);
    }

    private IEnumerator PlayHandRoutine()
    {
        MenuCanvasPanel.SetActive(false);
        GameCanvasPanel.SetActive(true);
        AIRevealToken.ResetSprite();
        PlayerRevealToken.ResetSprite();

        yield return ShowToast($"Round Start!");

        Timer.SetActive(true);

        float timer = 0;
        float totalTime = World.PlayHandTime;
        TimerFill.fillAmount = 1;
        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            TimerFill.fillAmount = 1 - (timer / totalTime);
            yield return null;
        }
        TimerFill.fillAmount = 0;
        World.UpdateGameState(GameState.Reveal);
    }

    private IEnumerator RevealHandRoutine()
    {
        Timer.SetActive(false);
        AIRevealToken.Init(World.AIElement, World.ElementMap.ElementIcons);
        PlayerRevealToken.Init(World.PlayerElement, World.ElementMap.ElementIcons);
        yield return new WaitForSeconds(1f);

        var winner = World.RoundWinner;
        var toastText = winner == WinState.Draw ? "Draw" : $"{winner} Wins!!";
        yield return ShowToast(toastText);

        AIScoreText.text = $"{World.AIScore}";
        PlayerScoreText.text = $"{World.PlayerScore}";
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
