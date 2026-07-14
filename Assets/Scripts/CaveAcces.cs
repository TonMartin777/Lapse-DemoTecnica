using UnityEngine;

public class CaveAccess : MonoBehaviour
{
    public new Collider collider;
    [SerializeField] private LayerMask playerLayer; // Permite ver una variable privada en el Inspector

    [Header("Horarios de la Cueva")] // Título de la sección en el Inspector

    [Tooltip("Hora a la que se abre la cueva")]
    public float nightStart = 110f;

    [Tooltip("Hora a la que se cierra la cueva")]
    public float morningStart = 15f;

    [Tooltip("Duración total del día (debe coincidir con el TimeManager)")]
    public float endOfDay = 180f;

    void Update() {
        bool playerInside = Physics.CheckSphere(transform.position, collider.bounds.extents.magnitude, playerLayer);
        float time = TimeManager.Instance.timeOfDay;

        // Desactiva el collider de noche
        bool isNight = (time >= nightStart && time <= endOfDay) || (time >= 0f && time <= morningStart);
        if (isNight && collider.enabled) { collider.enabled = false; }

        // Activa el collider de día si el jugador no está dentro
        bool isDay = (time >= morningStart && time < nightStart);
        if (isDay && !playerInside && !collider.enabled) { collider.enabled = true; }
    }
}