using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Info")]
    [SerializeField] private int initial_score = 3000; // The year the game starts on
    [SerializeField] private int initial_money = 50; // Starting amount of funds for the player
    [SerializeField] private float score_tick_delay = 1f; // Number of seconds it takes to earn 1 point (advance the calendar 1 year)
    private int score; // This is the "year" we display
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            ScoreUpdated?.Invoke(score);
        }
    }

    [Header("Spawn Values")]
    [SerializeField] private int max_number_of_mines;
    [SerializeField] private Vector2 mine_spawn_delay; // Number of seconds the game will wait before spawning a mine (x is low end, y is high end)
    [SerializeField] private int max_number_of_enemies;
    [SerializeField] private Vector2 enemy_spawn_delay; // Number of seconds the game will wait before spawning an enemy (x is low end, y is high end)

    [Header("Game Objects")]
    [SerializeField] private GameObject enemy_prefab;
    [SerializeField] private GameObject mine_task_prefab;
    private Objective objective; // This is the tree at the top of the planet. To change its starting health, change the tree's start_vitality value
    bool game_active = true;
    private List<MiningTask> mining_tasks; // Our list of active mining tasks
    private List<EnemyTask> enemy_tasks; // Our list of active enemy tasks

    private int money; // Player's current wallet
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            MoneyUpdated?.Invoke(money);
        }
    }

    /// <summary>
    /// These methods notify us when certain game events happen.
    /// In this class it's mostly for UI updating and controlling game flow.
    /// </summary>
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

    /// <summary>
    /// If there's already a GameManager in our game, this will destroy the current object so we don't have multiples.
    /// </summary>
    private void Singleton_InitializationFailedCallback()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// This method is called when the world tree is destroyed. It kicks off the end of the game.
    /// </summary>
    private void Objective_ObjectiveDestroyed()
    {
        game_active = false;
        StartCoroutine(GameEndRoutine());
    }

    /// <summary>
    /// This function is called when a mining task is finished.
    /// </summary>
    private void MiningTask_TaskComplete(object sender, EventArgs args)
    {
        MiningTask task = sender as MiningTask;

        task.Unsubscribe_TaskCompleted(MiningTask_TaskComplete);
        mining_tasks.Remove(task);
    }

    /// <summary>
    /// This function is called when an enemy is defeated.
    /// </summary>
    private void EnemyTask_TaskComplete(object sender, EventArgs args)
    {
        EnemyTask task = sender as EnemyTask;

        task.Unsubscribe_TaskCompleted(EnemyTask_TaskComplete);
        enemy_tasks.Remove(task);
    }

    /// <summary>
    /// Since the GameManager isn't destroyed between scenes, we use this method to reset the player's values when we start a new game.
    /// </summary>
    public void BeginNewGame()
    {
        score = initial_score;
        Money = initial_money;

        StartCoroutine(PlayGame());
    }

    /// <summary>
    /// Due to some weirdness with this project, this method is called by an outside class at the start of SampleScene.
    /// </summary>
    public void NewGameInitialization()
    {
        objective = FindObjectOfType<Objective>();

        if (objective != null)
        {
            objective.Subscribe_ObjectiveDestroyed(Objective_ObjectiveDestroyed);
        }
    }

    /// <summary>
    /// This is our main game loop.
    /// </summary>
    private IEnumerator PlayGame()
    {
        mining_tasks = new List<MiningTask>();
        enemy_tasks = new List<EnemyTask>();

        SoundManagerScript.PlayMusic("subterrafinalogg");

        Coroutine score_counter_routine = StartCoroutine(ScoreCounterRoutine());
        Coroutine spawn_mining_task_routine = StartCoroutine(SpawnMiningTaskRoutine());
        Coroutine spawn_enemy_routine = StartCoroutine(SpawnEnemyRoutine());

        while (game_active)
        {
            //HandleDebugInputs();
            yield return null;
        }

        StopCoroutine(score_counter_routine);
        GameEnd?.Invoke();
    }

    /// <summary>
    /// This stuff was just for testing and not neccesary for final game, so it's dummied out.
    /// </summary>
    private void HandleDebugInputs()
    {
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (mine_task_prefab != null)
            {
                Instantiate(mine_task_prefab, Vector3.zero, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f)));
            }
        }
    }

    /// <summary>
    /// This coroutine advances the score counter while we play.
    /// </summary>
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

    /// <summary>
    /// This coroutine spawns new mines if we haven't reached the maximum
    /// </summary>
    private IEnumerator SpawnMiningTaskRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(mine_spawn_delay.x, mine_spawn_delay.y));
            if (mining_tasks.Count < max_number_of_mines)
            {
                if (mine_task_prefab != null)
                {
                    MiningTask task = Instantiate(mine_task_prefab, Vector3.zero, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f))).GetComponent<MiningTask>();
                    if (task != null)
                    {
                        mining_tasks.Add(task);
                        task.Subscribe_TaskCompleted(MiningTask_TaskComplete);
                    }
                }
            }
        }
    }

    /// <summary>
    /// This coroutine spawns new enemies if we haven't reached the maximum
    /// </summary>
    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(enemy_spawn_delay.x, enemy_spawn_delay.y));
            if (enemy_tasks.Count < max_number_of_enemies)
            {
                if (mine_task_prefab != null)
                {
                    EnemyTask task = Instantiate(enemy_prefab, Vector3.zero, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(90f, 270f))).GetComponent<EnemyTask>();
                    if (task != null)
                    {
                        enemy_tasks.Add(task);
                        task.Subscribe_TaskCompleted(EnemyTask_TaskComplete);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Anything we want to happen before going to the Game Over screen will happen here.
    /// </summary>
    private IEnumerator GameEndRoutine()
    {
        SoundManagerScript.StopMusic();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameOverScene");
    }

    /// <summary>
    /// If we have a duplicate GameManager, we need to destroy it.
    /// But first, we need to do some cleanup with our delegates to avoid memory leaks.
    /// </summary>
    protected override void OnDestroy()
    {
        Unsubscribe_InitializationFailedCallback(Singleton_InitializationFailedCallback);
        base.OnDestroy();
    }
}
