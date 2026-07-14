using UnityEngine;
using System.Collections;

public class BedTrigger : MonoBehaviour, IInteractable // Implementa IInteractable
{
    private bool isSleeping = false;
    public AudioSource sleepSound;

    // LLama a la corrutina de dormir cuando el jugador interactúa con la cama
    public void Interact() {
        if (!isSleeping) { StartCoroutine(SleepRoutine()); }
    }

    // Se ejecuta al dormir
    private IEnumerator SleepRoutine() {
        PlayerMovement movementScript = FindAnyObjectByType<PlayerMovement>(); if (movementScript != null) movementScript.enabled = false;
        sleepSound.Play();
        yield return StartCoroutine(UIManager.Instance.FadeToBlack(2f));
        TimeManager.Instance.SkipToNextDay();
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(UIManager.Instance.FadeFromBlack(2f));

        if (movementScript != null) { movementScript.enabled = true; }
    }
}