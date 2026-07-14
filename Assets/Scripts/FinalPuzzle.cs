using UnityEngine;
using System.Collections;

public class FinalPuzzle : MonoBehaviour
{
    [Header("Configuración del Sol")]
    public Light sunLight;
    [Tooltip("1 es mirar exactamente al centro")]
    public float precision = 0.98f;

    [Header("Configuración del Tiempo")]
    public float horaCorrectaInicio = 14f;
    public float horaCorrectaFin = 15f;
    public float fadeDuration = 3f;

    private bool playerInside = false;
    private bool isLookingAtSun = false;
    private bool puzzleSolved = false;

    void Update() {
        if (puzzleSolved || !playerInside) { return; }

        bool correctTime = TimeManager.Instance.timeStopped && TimeManager.Instance.timeOfDay >= horaCorrectaInicio && TimeManager.Instance.timeOfDay <= horaCorrectaFin;
        
        if (correctTime) { // Comprueba si el tiempo esta parado en el momento correcto
            
            // Comprueba si el jugador mira al sol
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 sunDirection = -sunLight.transform.forward;
            float dot = Vector3.Dot(cameraForward.normalized, sunDirection.normalized);

            // Se muestra el prompt
            isLookingAtSun = dot > precision;
            UIManager.Instance.ShowInteractPrompt(isLookingAtSun);

            // Comprueba si se pulsa Q para iniciar la corutina
            if (isLookingAtSun && Input.GetKeyDown(KeyCode.Q)) {
                puzzleSolved = true;
                UIManager.Instance.ShowInteractPrompt(false);
                StartCoroutine(SolvePuzzleRoutine());
            }
        } else { // Si avanza el tiempo o no es la hora se oculta el prompt

            UIManager.Instance.ShowInteractPrompt(false);
        }
    }

    // Comprueba si el jugador está en el trigger
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) { playerInside = true; }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInside = false;
            UIManager.Instance.ShowInteractPrompt(false);
        }
    }

    // Se ejecuta al ineractuar
    private IEnumerator SolvePuzzleRoutine()
    {
        // Bloquea todo
        if (UIManager.Instance.player != null) {
            UIManager.Instance.player.enabled = false;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        AudioListener.volume = 0f;

        // Animación
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(UIManager.Instance.FadeToWhite(fadeDuration));
        yield return new WaitForSeconds(3f);

        // Salir al menú principal
        UIManager.Instance.LoadMainMenu();
    }   
}