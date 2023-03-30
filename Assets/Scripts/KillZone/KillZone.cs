using UnityEngine;

public class KillZone : MonoBehaviour{
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>() == null) return;
        if (!GameManager.Instance.IsGame) return;
        GameManager.Instance.GameOver();
    }
}
