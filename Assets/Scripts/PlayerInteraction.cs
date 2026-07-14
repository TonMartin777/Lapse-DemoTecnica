using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Camera playerCamera;
    public float interactDistance = 3f;
    private IInteractable currentInteractable;

    void Start() { playerCamera = Camera.main; }

    void Update() {
        CheckForInteractable();
        // Al pulsar Q, interactua
        if (currentInteractable != null && Input.GetKeyDown(KeyCode.Q)) {
            currentInteractable.Interact();
            UIManager.Instance.ShowInteractPrompt(false);
        }
    }

    // Comprueba si se puede interactuar
    void CheckForInteractable() {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance)) {
            if (hit.collider.CompareTag("Interact")) {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                // Si el objeto golpeado tiene un componente IInteractable, lo guardamos y mostramos la UI
                if (interactable != null) {
                    if (currentInteractable != interactable) {
                        currentInteractable = interactable;
                        UIManager.Instance.ShowInteractPrompt(true);
                    }
                    return;
                }
            }
        }

        // Si el rayo no golpea nada válido, se oculta la UI
        if (currentInteractable != null)
        {
            currentInteractable = null;
            UIManager.Instance.ShowInteractPrompt(false);
        }
    }
}