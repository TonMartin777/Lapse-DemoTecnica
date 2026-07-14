using UnityEngine;

public class PuzzleMovimiento : MonoBehaviour
{
    [Header("Tamaño")]
    public float minScale = 1f;
    public float maxScale = 5f;

    [Tooltip("Velocidad")]
    public float speed = 1f;

    private float timeCounter = 0f;

    // Tamaño máximo de la roca al inicio
    void Start() {
        timeCounter = maxScale - minScale;
        transform.localScale = new Vector3(maxScale, maxScale, maxScale);
    }

    // Si el tiempo está detenido, cambia el tamaño de la roca
    void Update() {
        if (TimeManager.Instance.timeStopped) {
            timeCounter += Time.deltaTime * speed;
            float scaleValue = Mathf.PingPong(timeCounter, maxScale - minScale) + minScale;
            transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        }
    }
}