using UnityEngine;

public class CarDestroyer : MonoBehaviour{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<Car>() != null) {
            other.gameObject.GetComponent<Car>().DestroyCar();
        }
    }
}
