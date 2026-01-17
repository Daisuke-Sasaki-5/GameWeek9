using System;
using System.Collections;
using UnityEngine;

public class FireBallSpawn : MonoBehaviour
{
    [Header("生成設定")]
    [SerializeField] private GameObject redFireBallPrefab;
    [SerializeField] private GameObject blueFireBallPrefab;

    [SerializeField] private float minspwanInterval = 5.0f;
    [SerializeField] private float maxspawnInterval = 7.0f;
    [SerializeField] private float spwanOffset = -15f;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // ゲーム開始してなければ待つ
            if (GameManager.instance == null || !GameManager.instance.isGameStarted)
            {
                yield return null;
                continue;
            }

            SpawnFireBall();

            // 5～7秒の間でランダムにスポーン時間が選ばれる
            float interval = UnityEngine.Random.Range(minspwanInterval, maxspawnInterval);
            yield return new WaitForSeconds(interval);
    }
}

    private void SpawnFireBall()
    {
        // 画面右端
        Vector3 rightEdge = cam.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));

        Vector3 spawnPos = new Vector3(rightEdge.x + 1f, spwanOffset, 0f);

        GameObject prefab = UnityEngine.Random.value < 0.5f ? redFireBallPrefab: blueFireBallPrefab;

        GameObject obj = GameObject.Instantiate(prefab, spawnPos, Quaternion.identity);
        
        FireBall fireBall = obj.GetComponent<FireBall>();
        fireBall.Init(Vector2.left);
    }
}
