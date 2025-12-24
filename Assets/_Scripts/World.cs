using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public GameState GameState
    {
        get
        {
            return _gameState;
        }
        set
        {
            _gameState = value;
            GameEventHandler.OnGameStateChange?.Invoke(_gameState);
        }
    }
    private GameState _gameState;

    public int PlayerHighScore
    {
        get => PlayerPrefs.GetInt("HighScore", 0);
        set => PlayerPrefs.SetInt("HighScore", value);
    }

    [NonSerialized] public GameElements PlayerElement;
    [NonSerialized] public GameElements AIElement;

    [NonSerialized] public string RoundWinner;
    [NonSerialized] public int AIScore;
    [NonSerialized] public int PlayerScore;

    public ElementMapSO ElementMap;
    public float PlayHandTime = 2f;

    private void Awake()
    {
        GameEventHandler.OnPlayerSelectElement += OnPlayerSelectElement;
    }

    private void OnPlayerSelectElement(GameElements element)
    {
        PlayerElement = element;
    }

    private void Start()
    {
        //Read Saves
        GameState = GameState.Menu;
        GameEventHandler.OnWorldInit?.Invoke(this);
    }

    public void UpdateGameState(GameState gameState)
    {
        if (gameState == GameState.Menu)
        {

        }
        if (gameState == GameState.Play)
        {
            RoundWinner = string.Empty;
            PlayerElement = GameElements.None;
            AIElement = GameElements.None;
            AIElement = SelectAIElement();
        }
        if (gameState == GameState.Reveal)
        {
            RoundWinner = SelectGameWinner();
            if (RoundWinner == "AI")
            {
                AIScore++;
            }
            else
            {
                PlayerScore++;
            }

            if (PlayerScore > PlayerHighScore)
            {
                PlayerHighScore = PlayerHighScore;
            }
        }
        if (gameState == GameState.PostGame)
        {
            if (RoundWinner == "AI")
            {
                GameEnd();
            }
            else
            {
                GameStart();
            }
            return;
        }

        GameState = gameState;
    }

    public void GameStart()
    {
        UpdateGameState(GameState.Play);
    }

    public void GameEnd()
    {
        UpdateGameState(GameState.Menu);
    }

    private GameElements SelectAIElement()
    {
        var values = Enum.GetValues(typeof(GameElements));
        return (GameElements)values.GetValue(UnityEngine.Random.Range(0, values.Length - 1));
    }

    private string SelectGameWinner()
    {
        if (PlayerElement == GameElements.None)
        {
            return "AI";
        }

        ElementWinMap map = ElementMap.ElementWins.Find(x => x.Element == PlayerElement);

        if (map.WinsAgainst.Contains(AIElement))
        {
            return "Player";
        }

        return "AI";
    }
}

public enum GameState
{
    Menu,
    Play,
    Reveal,
    PostGame
}

public enum GameElements
{
    Rock,
    Paper,
    Scissors,
    Lizard,
    Spock,
    None
}
