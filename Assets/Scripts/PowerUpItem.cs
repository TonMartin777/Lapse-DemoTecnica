using UnityEngine;
public enum PowerUpType { Clock, Lapse } // Enum con las mejoras

public class PowerUpItem : MonoBehaviour
{
    [Header("Configuración")]
    public PowerUpType type;
    public CanvasGroup infoPanel;

    [Header("Animación")]
    public float rotationSpeed = 90f;
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 2f;
    private Vector3 startPos;

    private void Start() { startPos = transform.position; }

    // Movimiento del objeto
    private void Update() {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    // Al recoger el power-up
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {

            // Llama a PlayerState para desbloquearlo
            if (type == PowerUpType.Clock) PlayerState.Instance.UnlockClock();
            else if (type == PowerUpType.Lapse) PlayerState.Instance.UnlockLapse();

            // Llama al UIManager para mostrar el panel de información
            UIManager.Instance.StartCoroutine(UIManager.Instance.ShowUnlockPanel(infoPanel));

            // Sonido de recogida
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null && audio.clip != null) {
                AudioSource.PlayClipAtPoint(audio.clip, transform.position);
            }

            Destroy(gameObject);
        }
    }
}