using UnityEngine;

public class CameraController : MonoBehaviour{
    [SerializeField]
    private float offset = 0f;
    [SerializeField]
    private float camSpeed = 0.2f;
    private Vector3 camPos;
    private void Start() {
        camPos = transform.position;
    }
    void Update(){
        camPos.z = GameManager.Instance.CurrentYLevel - offset;
        transform.position = Vector3.Slerp(transform.position, camPos, camSpeed * Time.deltaTime);
    }
}
