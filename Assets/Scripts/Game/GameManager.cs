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

    [SerializeField]
    private float pushDelta;
    private float lastPush = 0;

    public GameObject Player { get => player; }
    public int CurrentYLevel { get => currentYLevel; set => currentYLevel = value; }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this);
        } else {
            _instance = this;
        }
    }

    private void Update() {
        if (Time.time - lastPush > pushDelta) {
            lastPush = Time.time;
            currentYLevel++;
            ChunkManager.Instance.UpdateChunks();
        }
    }
}
