using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InteractableButton : MonoBehaviour, IInteractable // Implementa IInteractable
{
    [Header("Configuración del Botón")]
    public UnityEvent onInteract;
    public Vector3 pushOffset = new Vector3(0, -0.1f, 0);
    public AudioSource audioSource;

    private bool isAnimating = false;

    // Animación e inicio corrutina
    public void Interact() {
        if (!isAnimating) {
            if (audioSource != null) { audioSource.Play(); }
            StartCoroutine(AnimateAndTrigger());
        }
    }

    private IEnumerator AnimateAndTrigger()
    {
        isAnimating = true;
        Vector3 originalPos = transform.localPosition;

        // Hundir botón
        transform.localPosition = originalPos + pushOffset;

        // Lanza el evento y ejecuta todas las funciones suscritas a él
        onInteract.Invoke();

        yield return new WaitForSeconds(0.2f);

        // Posición original del botón
        transform.localPosition = originalPos;
        isAnimating = false;
    }
}
