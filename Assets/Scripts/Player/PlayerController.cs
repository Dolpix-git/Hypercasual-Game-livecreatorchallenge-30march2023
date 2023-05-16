using UnityEngine;

public class PlayerController : MonoBehaviour{
    [SerializeField]
    private float moveDelta = 0.5f;
    private float lastMove;

    [SerializeField] private GameObject visual;

    private void Awake() {
        CharacterInputManager.Instance.OnUp += Instance_OnUp;
        CharacterInputManager.Instance.OnDown += Instance_OnDown;
        CharacterInputManager.Instance.OnLeft += Instance_OnLeft;
        CharacterInputManager.Instance.OnRight += Instance_OnRight;
    }

    private void OnDestroy() {
        CharacterInputManager.Instance.OnUp -= Instance_OnUp;
        CharacterInputManager.Instance.OnDown -= Instance_OnDown;
        CharacterInputManager.Instance.OnLeft -= Instance_OnLeft;
        CharacterInputManager.Instance.OnRight -= Instance_OnRight;
    }

    private void Update() {
        if (Time.time - lastMove > moveDelta && CharacterInputManager.Instance.Movement.magnitude != 0) {
            if (CharacterInputManager.Instance.Movement.x != 0) {
                MoveCharacter(new Vector3(CharacterInputManager.Instance.Movement.x, 0, 0).normalized);
            }
            if (CharacterInputManager.Instance.Movement.y != 0) {
                MoveCharacter(new Vector3(0, 0, CharacterInputManager.Instance.Movement.y).normalized);
            }
        }
        if (transform.position.z > GameManager.Instance.score) {
            GameManager.Instance.ChangeScore((int)transform.position.z);
        }
    }

    private void Instance_OnRight() {
        MoveCharacter(new Vector3(1, 0, 0));
    }

    private void Instance_OnLeft() {
        MoveCharacter(new Vector3(-1, 0, 0));
    }

    private void Instance_OnDown() {
        MoveCharacter(new Vector3(0, 0, -1));
    }

    private void Instance_OnUp() {
        MoveCharacter(new Vector3(0,0,1));
    }

    private void MoveCharacter(Vector3 vect) {
        if (!GameManager.Instance.IsGame) return;
        lastMove = Time.time;
        Vector3 newVect = transform.position + vect;
        if (ChunkManager.Instance.CheckPosition((int)newVect.x, (int)newVect.z)) {

            visual.transform.rotation = Quaternion.LookRotation(transform.position - newVect, Vector3.up);
            transform.position = newVect;
        }
    }
}