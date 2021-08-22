using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int score = 3000; // This is the "year" we display
    [SerializeField] private float score_tick_delay = 1f;

    [Header("Game Objects")]
    [SerializeField] private GameObject enemy_prefab;
    [SerializeField] private GameObject mine_task_prefab;

    private int money;
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            MoneyUpdated?.Invoke(money);
        }
    }

    #region Delegates and Subscriber Methods

    private Action<int> MoneyUpdated;
    public void Subscribe_MoneyUpdated(Action<int> sub) { MoneyUpdated += sub; }
    public void Unsubscribe_MoneyUpdated(Action<int> sub) { MoneyUpdated -= sub; }

    private Action<int> ScoreUpdated;
    public void Subscribe_ScoreUpdated(Action<int> sub) { ScoreUpdated += sub; }
    public void Unsubscribe_ScoreUpdated(Action<int> sub) { ScoreUpdated -= sub; }

    private Action GameEnd;
    public void Subscribe_GameEnd(Action sub) { GameEnd += sub; }
    public void Unsubscribe_GameEnd(Action sub) { GameEnd -= sub; }

    #endregion

    protected override void Awake()
    {
        Subscribe_InitializationFailedCallback(Singleton_InitializationFailedCallback);

        base.Awake();
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        BeginNewGame();
    }

    private void Singleton_InitializationFailedCallback()
    {
        Destroy(gameObject);
    }

    public void BeginNewGame()
    {
        score = 3000;
        money = 0;
        StartCoroutine(PlayGame());
    }

    private IEnumerator PlayGame()
    {
        Coroutine score_counter_routine = StartCoroutine(ScoreCounterRoutine());
        bool game_active = true;

        while (game_active)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                game_active = false;
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                money += 10;
                MoneyUpdated?.Invoke(money);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                StopAllCoroutines();
                SceneManager.LoadScene("GameOverScene");
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (enemy_prefab != null)
                {
                    Instantiate(enemy_prefab, Vector3.zero, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(90f, 270f)));
                }
            }

            yield return null;
        }

        StopCoroutine(score_counter_routine);
        GameEnd?.Invoke();
    }

    private IEnumerator ScoreCounterRoutine()
    {
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer >= score_tick_delay)
            {
                score++;
                ScoreUpdated?.Invoke(score);
                timer -= score_tick_delay;
            }

            yield return null;
        }
    }

    protected override void OnDestroy()
    {
        Unsubscribe_InitializationFailedCallback(Singleton_InitializationFailedCallback);
        base.OnDestroy();
    }
}
