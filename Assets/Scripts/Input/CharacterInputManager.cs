using System;
using UnityEngine;

public class CharacterInputManager : MonoBehaviour{
    #region Singleton Manager.
    private static CharacterInputManager _instance;
    public static CharacterInputManager Instance {
        get {
            if (_instance is null) {
                _instance = FindObjectOfType<CharacterInputManager>();
                if (_instance is null) {
                    var obj = Instantiate(new GameObject("CharacterInputManager"));
                    _instance = obj.AddComponent<CharacterInputManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    #region Private.
    private PlayerControl playerControl;
    private Vector2 movement;
    #endregion

    #region Getters Setters.
    public Vector2 Movement { get => movement; set => movement = value; }
    #endregion

    #region Events.
    public event Action OnUp;
    public event Action OnDown;
    public event Action OnLeft;
    public event Action OnRight;
    #endregion

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this);
        } else {
            _instance = this;
        }

        playerControl = new PlayerControl();
        playerControl.Character.Enable();

        playerControl.Character.Up.performed += Up_performed;
        playerControl.Character.Down.performed += Down_performed;
        playerControl.Character.Left.performed += Left_performed;
        playerControl.Character.Right.performed += Right_performed;
    }

    private void OnDestroy() {
        playerControl.Character.Up.performed -= Up_performed;
        playerControl.Character.Down.performed -= Down_performed;
        playerControl.Character.Left.performed -= Left_performed;
        playerControl.Character.Right.performed -= Right_performed;
    }

    private void Update() {
        movement = playerControl.Character.Move.ReadValue<Vector2>();
    }

    private void Right_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnRight?.Invoke();
    }

    private void Left_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnLeft?.Invoke();
    }

    private void Down_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnDown?.Invoke();
    }

    private void Up_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnUp?.Invoke();
    }
}
