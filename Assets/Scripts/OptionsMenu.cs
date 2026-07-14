using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Interfaz")]
    public GameObject optionsPanel;

    [Header("Referencias")]
    public PlayerMovement player;

    [Header("Sliders")]
    public Slider sensitivitySlider;
    public Slider volumeSlider;

    // Valores por defecto para reiniciar
    private float defaultSensitivity = 2f;
    private float defaultVolume = 1f;

    void Start() {
        // Apagar panel de opciones al inicio
        if (optionsPanel != null) { optionsPanel.SetActive(false); }

        // Settear sliders
        sensitivitySlider.value = player.mouseSensitivity;
        volumeSlider.value = AudioListener.volume;

        // Activar listeners de sliders
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Métodos para sliders
    public void SetSensitivity(float value) { player.mouseSensitivity = value; }

    public void SetVolume(float value) { AudioListener.volume = value; }

    // Al pulsar Opciones
    public void OpenOptions() { optionsPanel.SetActive(true); }

    // Al pulsar Cerrar
    public void CloseOptions() { optionsPanel.SetActive(false); }

    // Al pulsar Reiniciar
    public void ResetToDefaults() {
        sensitivitySlider.value = defaultSensitivity;
        volumeSlider.value = defaultVolume;
    }
}