using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [Header("生成設定")]
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private float startInterval = 1.5f;
    [SerializeField] private float minInterval = 0.3f;
    [SerializeField] private float intervalDecreaseRate = 0.02f;

    private float elapsedTime = 0f;
    private bool isSpawning = false;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // ゲーム開始前は何もしない
        if (!GameManager.instance || !IsGameStarted())return;

        if(!isSpawning)
        {
            StartCoroutine(SpawnRoutine());
        }

        elapsedTime += Time.deltaTime;
    
    }

    private bool IsGameStarted()
    {
        return GameManager.instance.isGameStarted;
    }

    IEnumerator SpawnRoutine()
    {
        isSpawning = true;

        float interval = Mathf.Max(minInterval,startInterval - elapsedTime * intervalDecreaseRate);

        SpawnRock();

        yield return new WaitForSeconds(interval);
        isSpawning = false;
    }

    void SpawnRock()
    {
        Vector3 leftTop = cam.ViewportToWorldPoint(new Vector3(0f, 1f, 0f));
        Vector3 rightTop = cam.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

        float x = UnityEngine.Random.Range(leftTop.x, rightTop.x);
        float y = leftTop.y + 1f; // 少し画面外から

        Instantiate(rockPrefab,new Vector3(x,y,0f), Quaternion.identity);
    }
}
