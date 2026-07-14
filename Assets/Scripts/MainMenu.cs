using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Paneles de la Interfaz")]
    public GameObject mainMenuPanel;
    public Image fadePanel;
    public float fadeDuration = 1f;

    [Header("Configuración del Carrusel")]
    public RawImage backgroundImage;
    public Texture[] backgroundImages;
    public float timePerImage = 5f;

    private int currentBackgroundIndex;

    private void Start() {
        Application.targetFrameRate = 60;

        // Limpieza de estado
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);

        // Panel negro para el fade
        if (fadePanel != null) {
            fadePanel.gameObject.SetActive(true);
            fadePanel.color = new Color(0, 0, 0, 0);
            fadePanel.raycastTarget = false;
        }

        // Iniciar carrusel
        if (backgroundImages.Length > 0 && backgroundImage != null)
        {
            StartCoroutine(BackgroundCarouselRoutine());
        }
    }

    // Al pulsar Jugar
    public void PlayGame() { StartCoroutine(FadeToBlackAndLoad("Game")); }

    // AL pulsar Salir
    public void QuitGame() { Application.Quit(); }

    // Iniciar el juego
    private IEnumerator FadeToBlackAndLoad(string sceneName)
    {
        float timer = 0f;

        // Fade a negro
        while (timer < fadeDuration) {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadePanel.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        fadePanel.color = new Color(0f, 0f, 0f, 1f);

        // Carga el juego de forma asíncrona para no bloquear el hilo principal
        SceneManager.LoadSceneAsync(sceneName);
    }

    // Carrusel de imagenes de fondo
    private IEnumerator BackgroundCarouselRoutine()
    {
        currentBackgroundIndex = Random.Range(0, backgroundImages.Length);
        backgroundImage.texture = backgroundImages[currentBackgroundIndex];

        while (true) {
            yield return new WaitForSeconds(timePerImage);

            float timer = 0f;
            while (timer < fadeDuration) {
                timer += Time.deltaTime;
                backgroundImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, timer / fadeDuration));
                yield return null;
            }

            currentBackgroundIndex = (currentBackgroundIndex + 1) % backgroundImages.Length;
            backgroundImage.texture = backgroundImages[currentBackgroundIndex];

            timer = 0f;
            while (timer < fadeDuration) {
                timer += Time.deltaTime;
                backgroundImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, timer / fadeDuration));
                yield return null;
            }
        }
    }
}