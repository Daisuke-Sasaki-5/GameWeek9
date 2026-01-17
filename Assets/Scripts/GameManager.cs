using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework.Interfaces;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public float SurvivalTime { get; private set; } 

    [Header("制限時間")]
    [SerializeField] private float timeLimit = 0f; // 制限時間
    [SerializeField] private TMP_Text timetext;
    private float timer;
    private bool isGameOver = false;

    [Header("スタートUI")]
    [SerializeField] private TextMeshProUGUI ClickToStartText;
    [SerializeField] private TextMeshProUGUI readytext;
    [SerializeField] private TextMeshProUGUI gotext;

    private Coroutine startRoutine;

    private bool isWaitingForInput = true;
    private bool isGameStarted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitGame();
    }

    /// <summary>
    ///  ゲームの初期化
    /// </summary>
    private void InitGame()
    {
        // 現在のシーンをチェック
        if (SceneManager.GetActiveScene().name != "Game") return;

        timer = timeLimit;
        isGameOver = false;
        isGameStarted = false;
        isWaitingForInput = true;

        UpdateTimeUI();

        // UI初期化
        if (timetext != null) timetext.gameObject.SetActive(false);
        if (readytext != null) readytext.gameObject.SetActive(false);
        if (gotext != null) gotext.gameObject.SetActive(false);
        if(ClickToStartText != null) ClickToStartText.gameObject.SetActive(true);

        Time.timeScale = 0f; // 完全停止

        // 既存コルーチンがあれば停止
        if (startRoutine != null) StopCoroutine(startRoutine);
    }

    private void Update()
    {
        // ゲーム開始待ち
        if (isWaitingForInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isWaitingForInput = false;
                if (ClickToStartText != null) ClickToStartText.gameObject.SetActive(false);
                startRoutine = StartCoroutine(StartGameRoutine());
            }
            return;
        }

        if(!isGameStarted || isGameOver) return;

        timer += Time.deltaTime;
        SurvivalTime = timer;
        UpdateTimeUI();
        
    }

    // ==== READY/GO 演出 ====
    private IEnumerator StartGameRoutine()
    {
        // 時間停止
        Time.timeScale = 0f;

        if (FadeManager.instance != null)
        {
            while (!FadeManager.instance.IsFadeComplete)
            {
                yield return null;
            }
        }

        if (readytext != null)
        {
            readytext.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(1f);
            readytext.gameObject.SetActive(false);
        }

        if (gotext != null)
        {
            gotext.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(1f);
            gotext.gameObject.SetActive(false);
        }

        if (timetext != null) timetext.gameObject.SetActive(true);

        // ゲーム開始
        Time.timeScale = 1f;
        isGameStarted = true;
    }

    // ==== 時間表示 ====
    private void UpdateTimeUI()
    {
        if (timetext != null)
        {
            timetext.text = "Time:" + timer.ToString("F1");
        }
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("ゲームオーバー");
        FadeManager.instance.FadeToScene("Result");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    // ==== シーンリセット ====
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            InitGame();
        }
        else
        {
            if (timetext != null) timetext.gameObject.SetActive(false);
            if (readytext != null) readytext.gameObject.SetActive(false);
            if (gotext != null) gotext.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
