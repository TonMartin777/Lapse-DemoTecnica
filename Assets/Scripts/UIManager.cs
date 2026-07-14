using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Singleton
    public static UIManager Instance { get; private set; }

    [Header("Menus")]
    public CanvasGroup pauseMenu;
    public OptionsMenu optionsMenu;
    private bool isPaused = false;
    public PlayerMovement player;

    [Header("HUD")]
    public GameObject interactPrompt;
    public TextMeshProUGUI clockText;

    [Header("Panels")]
    public GameObject signPanel;
    public TextMeshProUGUI signText;

    [Header("Fades")]
    public Image fadeImage;
    public Image finalFadeImage;

    [HideInInspector]
    public bool isReading = false;

    // Solo una instancia
    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Quitamos la UI al inicio
    private void Start() {
        Application.targetFrameRate = 60;
        if (interactPrompt != null) interactPrompt.SetActive(false);
        if (signPanel != null) signPanel.SetActive(false);
        ResumeGame();
    }

    // Detecta la tecla ESC para abrir/cerrar la pausa
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (optionsMenu != null && optionsMenu.optionsPanel.activeSelf) {
                optionsMenu.CloseOptions();
            } else if (!isPaused) {
                PauseGame();
            } else {
                ResumeGame();
            }
        }
    }

    // Parar juego
    public void PauseGame() {
        isPaused = true;
        AudioListener.pause = true;
        pauseMenu.alpha = 1;
        pauseMenu.interactable = true;
        pauseMenu.blocksRaycasts = true;

        Time.timeScale = 0f;
        if (player != null) player.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Reanudar juego
    public void ResumeGame()
    {
        // Si el menú de opciones existe y su panel está encendido, lo cerramos.
        if (optionsMenu != null && optionsMenu.optionsPanel.activeSelf) {
            optionsMenu.CloseOptions();
        }

        isPaused = false;
        pauseMenu.alpha = 0;
        pauseMenu.interactable = false;
        pauseMenu.blocksRaycasts = false;

        Time.timeScale = 1f;
        AudioListener.pause = false;

        // Si el jugador está leyendo un cartel, el jugador sigue bloqueado pese salir de la pausa
        if (!isReading) {
            if (player != null) { player.enabled = true; }
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            if (player != null) { player.enabled = false; }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void LoadMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowInteractPrompt(bool show) {
        if (interactPrompt != null && interactPrompt.activeSelf != show) {
            interactPrompt.SetActive(show);
        }
    }

    // Abrir y cerrar cartel
    public void OpenSign(string text) {
        signPanel.SetActive(true);
        signText.text = text;
        isReading = true;
        Time.timeScale = 0f;
        if (player != null) player.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseSign() {
        signPanel.SetActive(false);
        isReading = false;
        Time.timeScale = 1f;
        if (player != null) player.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Fundidos de pantalla
    public IEnumerator FadeToBlack(float duration) {
        float t = 0;
        while (t < duration) {
            t += Time.unscaledDeltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(t / duration));
            yield return null;
        }
    }

    public IEnumerator FadeFromBlack(float duration) {
        float t = 0;
        while (t < duration) {
            t += Time.unscaledDeltaTime;
            fadeImage.color = new Color(0, 0, 0, 1 - Mathf.Clamp01(t / duration));
            yield return null;
        }
    }

    public IEnumerator FadeToWhite(float duration) {
        float t = 0;
        while (t < duration) {
            t += Time.unscaledDeltaTime;
            finalFadeImage.color = new Color(1, 1, 1, Mathf.Clamp01(t / duration));
            yield return null;
        }
    }

    // Panel de Power-ups
    public IEnumerator ShowUnlockPanel(CanvasGroup panel) {
        Time.timeScale = 0f;
        if (player != null) player.enabled = false;
        Cursor.lockState = CursorLockMode.None;

        panel.gameObject.SetActive(true);

        // Fundido de entrada
        float t = 0f;
        float duration = 1f;
        while (t < duration) {
            t += Time.unscaledDeltaTime;
            panel.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }
        panel.alpha = 1f;

        // Esperar a que el jugador pulse Q
        while (!Input.GetKeyDown(KeyCode.Q)) {
            yield return null;
        }

        // Fundido de salida
        t = 0f;
        while (t < duration) {
            t += Time.unscaledDeltaTime;
            panel.alpha = Mathf.Lerp(1f, 0f, t / duration);
            yield return null;
        }
        panel.alpha = 0f;
        panel.gameObject.SetActive(false);

        // Devolvemos el juego a la normalidad
        Time.timeScale = 1f;
        if (player != null) player.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Power-up del reloj
    public void UpdateClockUI(float timeOfDay) {
        if (clockText != null) {
            int minutes = Mathf.FloorToInt(timeOfDay / 60f);
            int seconds = Mathf.FloorToInt(timeOfDay % 60f);

            clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}