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
    private bool isGamePaused = false;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private readonly float gamePlayingTimerMax = 20f;

    public event Action OnStateChanged;
    public event Action OnGamePaused;
    public event Action OnGameUnpaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        state = GameState.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.CountdownToStart:
                HandleCountdownToStartState();
                break;
            case GameState.GamePlaying:
                HandleGamePlayingState();
                break;
            case GameState.GameOver:
                break;
        }
    }

    public bool IsCountdownToStartActive()
    {
        return state == GameState.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public bool IsGamePlaying()
    {
        return state == GameState.GamePlaying;
    }

    public bool IsGameOverActive()
    {
        return state == GameState.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    private void HandleWaitingToStartState()
    {
        if (state == GameState.WaitingToStart)
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
            gamePlayingTimer = gamePlayingTimerMax;
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

    private void GameInput_OnInteractAction()
    {
        HandleWaitingToStartState();
    }

    private void GameInput_OnPauseAction()
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke();
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke();
        }
    }
}
