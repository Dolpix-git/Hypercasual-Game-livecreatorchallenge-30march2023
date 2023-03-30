using UnityEngine;

public class KillZone : MonoBehaviour{
    private void OnTriggerEnter(Collider other) {
        Debug.Log("DEATH");
    }
}
