using UnityEngine;
public class ElevatorCollider : MonoBehaviour
{
    public Elevator elevator;

    // Detecta si el jugador entra
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            elevator.jugadorDebajo = true;
        }
    }

    // Detecta si el jugador sale
    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            elevator.jugadorDebajo = false;
        }
    }
}