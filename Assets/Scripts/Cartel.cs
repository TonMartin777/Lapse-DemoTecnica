using UnityEngine;

public class Cartel : MonoBehaviour, IInteractable // Implementa IInteractable
{
    // Permite tener un espacio de texto más grande en el inspector para escribir el mensaje del cartel
    [TextArea(3, 10)]
    public string message;

    private bool isReading = false;

    // Si se esta leyendo se cierra, sino se abre
    public void Interact() {
        if (!isReading) { OpenSign(); }
        else { CloseSign(); }
    }

    // Llama al UIManager
    void OpenSign() {
        isReading = true;
        UIManager.Instance.OpenSign(message);
    }

    void CloseSign() {
        isReading = false;
        UIManager.Instance.CloseSign();
    }
}