using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Ruta")]
    public Transform puntoInferior;
    public Transform puntoSuperior;

    public float velocidad = 3f;

    [Header("Estado")]
    public bool isMoving = false;
    private bool estaArriba = false;
    [HideInInspector] public bool jugadorDebajo = false;

    public AudioSource audioSource;
    private CharacterController playerController;
    private Vector3 destinoActual;

    // Determinamos si el ascensor está arriba o abajo al inicio
    void Start() {
        float distArriba = Vector3.Distance(transform.position, puntoSuperior.position);
        float distAbajo = Vector3.Distance(transform.position, puntoInferior.position);
        estaArriba = distArriba < distAbajo;
    }

    // Mueve el ascensor al pulsar el botón
    public void activar() {
        if (!isMoving) {
            destinoActual = estaArriba ? puntoInferior.position : puntoSuperior.position;
            isMoving = true;
            if (audioSource != null) { audioSource.Play(); }
        }
    }

    // FixedUpdate se usa para física y movimiento constante
    void FixedUpdate()
    {
        if (!isMoving) { return; }

        // Si el tiempo está parado o el jugador está debajo mientras baja, se detiene
        if (TimeManager.Instance.timeStopped || (estaArriba && jugadorDebajo))
        {
            if (audioSource != null && audioSource.isPlaying) { audioSource.Pause(); }
            return;
        }

        if (audioSource != null && !audioSource.isPlaying) { audioSource.UnPause(); }

        // Calcula la siguiente posición
        Vector3 posicionAnterior = transform.position;
        Vector3 nuevaPosicion = Vector3.MoveTowards(transform.position, destinoActual, velocidad * Time.fixedDeltaTime);
        Vector3 deltaMovimiento = nuevaPosicion - transform.position;


        if (deltaMovimiento.y > 0) { // Si sube, eleva al jugador primero
            if (playerController != null) { playerController.Move(deltaMovimiento); }
            transform.position = nuevaPosicion;

        } else { // Si baja, mueve al jugador después
            transform.position = nuevaPosicion;
            if (playerController != null) { playerController.Move(deltaMovimiento); }
        }

        // Comprueba si ha llegado al destino
        if (Vector3.Distance(transform.position, destinoActual) <= 0.001f) {
            transform.position = destinoActual;
            estaArriba = !estaArriba;
            isMoving = false;
            if (audioSource != null) { audioSource.Stop(); }
        }
    }

    // Detecta si el jugador entra al ascensor
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerController = other.GetComponent<CharacterController>();
        }
    }

    // Detecta si el jugador sale del ascensor
    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerController = null;
        }
    }
}