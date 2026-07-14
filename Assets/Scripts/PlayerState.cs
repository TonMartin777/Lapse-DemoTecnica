using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // Singleton que guarda estado del jugador
    public static PlayerState Instance { get; private set; }

    [Header("Mejoras")]
    public bool hasClock = false;
    public bool hasLapse = false;

    // Solo una insancia
    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Desbloquear mejoras
    public void UnlockClock() {
        hasClock = true;
        if (UIManager.Instance.clockText != null) {
            UIManager.Instance.clockText.gameObject.SetActive(true);
        }
    }

    public void UnlockLapse() { hasLapse = true; }
}
