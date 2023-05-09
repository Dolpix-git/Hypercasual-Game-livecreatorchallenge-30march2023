using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour{
    #region Singleton Pattern
    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance is null) {
                _instance = FindObjectOfType<GameManager>();
                if (_instance is null) {
                    var obj = Instantiate(new GameObject("GameManager"));
                    _instance = obj.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField]
    private GameObject player;
    private int currentYLevel = 0;

    public int score = 0;

    [SerializeField]
    private float pushDelta;
    private float lastPush = 0;

    public bool IsGame;

    public event Action OnGameStart;
    public event Action OnGameEnd;
    public event Action<int> OnScoreChange;

    public GameObject Player { get => player; }
    public int CurrentYLevel { get => currentYLevel; set => currentYLevel = value; }
    private Vector3 playerSpawn;
    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this);
        } else {
            _instance = this;
        }
    }
    private void Start() {
        playerSpawn = player.transform.position;
        StartGame();
    }
    private void Update() {
        if (Time.time - lastPush > pushDelta && IsGame) {
            lastPush = Time.time;
            currentYLevel++;
            ChunkManager.Instance.UpdateChunks();
        }
    }

    public void GameOver() {
        IsGame = false;
        StartCoroutine(WaitToStart());
        OnGameEnd?.Invoke();
    }
    IEnumerator WaitToStart() {
        yield return new WaitForSeconds(3);
        RestartGame();
    }
    public void RestartGame() {
        ChunkManager.Instance.ClearAllChunks();
        player.transform.position = playerSpawn;
        currentYLevel = 0;
        score = 0;
        StartGame();
    }
    public void StartGame() {
        IsGame = true;
        OnGameStart?.Invoke();
    }
    public void ChangeScore(int amount) {
        score = amount;
        OnScoreChange?.Invoke(amount);
    }
}
