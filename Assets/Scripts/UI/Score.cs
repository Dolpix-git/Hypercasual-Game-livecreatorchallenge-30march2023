using TMPro;
using UnityEngine;

public class Score : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI text;

    private void Awake() {
        GameManager.Instance.OnScoreChange += Instance_OnScoreChange;
    }

    private void Instance_OnScoreChange(int obj) {
        text.text = $"Score: {obj}";
    }
}
