using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private GameState state;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer = 10f;

    public event Action OnStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        state = GameState.WaitingToStart;
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.WaitingToStart:
                HandleWaitingToStartState();
                break;
            case GameState.CountdownToStart:
                HandleCountdownToStartState();
                break;
            case GameState.GamePlaying:
                HandleGamePlayingState();
                break;
            case GameState.GameOver:
                break;
        }

        Debug.Log($"Current Game State: {state}");
    }

    public bool IsGamePlaying()
    {
        return state == GameState.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == GameState.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    private void HandleWaitingToStartState()
    {
        waitingToStartTimer -= Time.deltaTime;

        if (waitingToStartTimer <= 0f)
        {
            state = GameState.CountdownToStart;
            OnStateChanged?.Invoke();
        }
    }

    private void HandleCountdownToStartState()
    {
        countdownToStartTimer -= Time.deltaTime;

        if (countdownToStartTimer <= 0f)
        {
            state = GameState.GamePlaying;
            OnStateChanged?.Invoke();
        }
    }

    private void HandleGamePlayingState()
    {
        gamePlayingTimer -= Time.deltaTime;

        if (gamePlayingTimer <= 0f)
        {
            state = GameState.GameOver;
            OnStateChanged?.Invoke();
        }
    }
}
