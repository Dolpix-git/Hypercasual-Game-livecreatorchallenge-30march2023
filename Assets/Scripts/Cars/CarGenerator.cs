using System.Collections.Generic;
using UnityEngine;

public class CarGenerator : MonoBehaviour{
    [SerializeField] 
    private GameObject car;
    [SerializeField]
    private float randomDeltaMin, randomDeltaMax;
    private float randomDelta, currentDelta;
    [SerializeField]
    private float carSpeedMin, carSpeedMax;
    private float carSpeed;

    public float direction;

    private List<GameObject> cars = new List<GameObject>();
    private void Start() {
        carSpeed = Random.Range(carSpeedMin, carSpeedMax);
    }

    void Update(){
        if (Time.time - currentDelta > randomDelta) {
            currentDelta = Time.time;
            randomDelta = Random.Range(randomDeltaMin, randomDeltaMax);
            GameObject newCar = Instantiate(car, transform.position,Quaternion.identity);
            newCar.GetComponent<Car>().speed = carSpeed;
            newCar.GetComponent<Car>().direction = direction;
            newCar.GetComponent<Car>().generator = this;
            cars.Add(newCar);
        }
    }
    private void OnDestroy() {
        GameObject[] carArray = cars.ToArray();
        for (int i = 0; i < carArray.Length; i++) {
            Destroy(carArray[i]);
        }
    }
    public void DestroyCar(GameObject car) {
        cars.Remove(car);
        Destroy(car);
    }
}
