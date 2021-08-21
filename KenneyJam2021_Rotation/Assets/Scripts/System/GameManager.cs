using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
    public int score = 3000; // This is the "year" we display
    [SerializeField] private float score_tick_delay = 1f;
    public int money;

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

    private void Start()
    {
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
}
