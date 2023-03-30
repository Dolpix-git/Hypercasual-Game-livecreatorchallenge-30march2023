using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGenerator : MonoBehaviour{
    [SerializeField] 
    private GameObject car;
    [SerializeField]
    private float randomDeltaMin, randomDeltaMax;
    private float randomDelta, currentDelta;

    private List<GameObject> cars = new List<GameObject>();

    // Update is called once per frame
    void Update(){
        if (Time.time - currentDelta > randomDelta) {
            randomDelta = Random.Range(randomDeltaMin, randomDeltaMax);
        }
    }
}
