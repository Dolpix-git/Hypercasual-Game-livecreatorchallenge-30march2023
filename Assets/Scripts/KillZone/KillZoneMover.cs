using UnityEngine;

public class KillZoneMover : MonoBehaviour{
    [SerializeField]
    private float offset = 0f;
    [SerializeField]
    private float camSpeed = 0.2f;
    private Vector3 camPos;
    private Vector3 spawn;
    private void Awake() {
        GameManager.Instance.OnGameStart += Instance_OnGameStart;
    }
    private void Start() {
        camPos = spawn = transform.position;
    }
    private void Instance_OnGameStart() {
        if (spawn == Vector3.zero) return;
        transform.position = spawn;
    }
    void Update() {
        camPos.z = GameManager.Instance.CurrentYLevel - offset;
        transform.position = Vector3.Slerp(transform.position, camPos, camSpeed * Time.deltaTime);
    }
}
