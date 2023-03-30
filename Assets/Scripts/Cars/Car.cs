using UnityEngine;

public class Car : MonoBehaviour {

    public float speed;    
    public float direction;
    public CarGenerator generator;

    // Update is called once per frame
    void Update(){
        transform.position += new Vector3(speed * direction, 0, 0) * Time.deltaTime;
    }

    public void DestroyCar() {
        generator.DestroyCar(gameObject);
    }
}
