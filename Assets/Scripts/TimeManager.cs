using UnityEngine;
using UnityEngine.Rendering;

public class TimeManager : MonoBehaviour
{
    // Singleton
    public static TimeManager Instance { get; private set; }

    [Header("Sun Settings")]
    public Light sunLight;
    public float dayLengthInSeconds = 180f;
    public float maxSunIntensity = 1f;

    [Header("Curva de Velocidad del Sol")]
    [Tooltip("Controla cuánto tarda el sol")]
    public AnimationCurve sunCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Header("Luz del Jugador")]
    public Light playerNightLight;
    public float maxNightLightIntensity = 1f;

    [Header("Gradientes (0% Noche | 50% Atardecer | 100% Día)")]
    public Gradient sunLightColor;
    public Gradient ambientLightColor;

    [Header("Gradientes del Skybox")]
    public Gradient skyTopColor;
    public Gradient skyBottomColor;
    public Gradient skyHorizonColor;
    public Gradient sunHaloColor;
    public Gradient sunDiscColor;

    [Header("Nombres internos del Shader")]
    public string topColorName = "_SkyGradientTop";
    public string bottomColorName = "_SkyGradientBottom";
    public string horizonColorName = "_HorizonLineColor";
    public string sunHaloColorName = "_SunHaloColor";
    public string sunDiscColorName = "_SunDiscColor";

    [Header("Time State")]
    public float timeOfDay = 0f;
    private float timeNormalized = 0f;
    public bool timeStopped = false;

    [Header("Efecto de Tiempo Parado")]
    public Volume timeStopVolume;
    public float timeStopTransitionSpeed = 3f;

    private Material skyboxClone;

    [Header("Sonidos del Tiempo")]
    public AudioSource timeMagicAudio;
    public AudioClip stopTimeClip;
    public AudioClip resumeTimeClip;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        Application.targetFrameRate = 60;
        if (RenderSettings.skybox != null) {
            skyboxClone = new Material(RenderSettings.skybox);
            RenderSettings.skybox = skyboxClone;
        }
        RenderSettings.ambientMode = AmbientMode.Flat;
    }

    void Update() {
        // Avanza el tiempo
        if (!timeStopped) {
            timeOfDay += Time.deltaTime;
            if (timeOfDay >= dayLengthInSeconds) { timeOfDay -= dayLengthInSeconds; }

            timeNormalized = timeOfDay / dayLengthInSeconds;

            // Mover el Sol
            float curveValue = sunCurve.Evaluate(timeNormalized);
            float angle = curveValue * 360f;
            sunLight.transform.rotation = Quaternion.Euler(angle, 0f, 0f);

            UpdateLighting();
        }

        // Activa/desactiva el tiempo
        if (PlayerState.Instance != null && PlayerState.Instance.hasLapse && Input.GetKeyDown(KeyCode.E)) {
            timeStopped = !timeStopped;

            if (timeStopped) {
                if (timeMagicAudio != null && stopTimeClip != null) { timeMagicAudio.PlayOneShot(stopTimeClip); }

                // Cambios audio
                if (Camera.main != null) {
                    AudioLowPassFilter filter = Camera.main.GetComponent<AudioLowPassFilter>();
                    if (filter != null) {
                        filter.enabled = true;
                        filter.cutoffFrequency = 800f;
                    }
                }
            } else {
                if (timeMagicAudio != null && resumeTimeClip != null) { timeMagicAudio.PlayOneShot(resumeTimeClip); }

                // Cambios audio
                if (Camera.main != null) {
                    AudioLowPassFilter filter = Camera.main.GetComponent<AudioLowPassFilter>();
                    if (filter != null) {
                        filter.cutoffFrequency = 22000f;
                    }
                }
            }
        }


        // Actualiza la UI del reloj
        if (PlayerState.Instance != null && PlayerState.Instance.hasClock) { UIManager.Instance.UpdateClockUI(timeOfDay); }

        // Efecto visual
        if (timeStopVolume != null) {
            float targetWeight = timeStopped ? 1f : 0f;
            timeStopVolume.weight = Mathf.MoveTowards(timeStopVolume.weight, targetWeight, Time.deltaTime * timeStopTransitionSpeed);
        }
    }

    // Actualiza iluminacion y skybox
    private void UpdateLighting() {
        float sunHeight = Vector3.Dot(-sunLight.transform.forward, Vector3.up);
        float gradientTime = (sunHeight + 1f) / 2f;

        sunLight.color = sunLightColor.Evaluate(gradientTime);
        float intensityTransition = Mathf.Clamp01(sunHeight * 5f);
        sunLight.intensity = Mathf.Lerp(0f, maxSunIntensity, intensityTransition);
        sunLight.enabled = (sunHeight > -0.05f);

        RenderSettings.ambientLight = ambientLightColor.Evaluate(gradientTime);

        if (playerNightLight != null) {
            playerNightLight.intensity = Mathf.Lerp(maxNightLightIntensity, 0f, intensityTransition);
        }

        // Actualiza el Skybox
        if (skyboxClone != null) {
            if (skyboxClone.HasProperty(topColorName)) skyboxClone.SetColor(topColorName, skyTopColor.Evaluate(gradientTime));
            if (skyboxClone.HasProperty(bottomColorName)) skyboxClone.SetColor(bottomColorName, skyBottomColor.Evaluate(gradientTime));
            if (skyboxClone.HasProperty(horizonColorName)) skyboxClone.SetColor(horizonColorName, skyHorizonColor.Evaluate(gradientTime));
            if (skyboxClone.HasProperty(sunHaloColorName)) skyboxClone.SetColor(sunHaloColorName, sunHaloColor.Evaluate(gradientTime));
            if (skyboxClone.HasProperty(sunDiscColorName)) skyboxClone.SetColor(sunDiscColorName, sunDiscColor.Evaluate(gradientTime));
        }
    }

    public void SkipToNextDay() {
        timeOfDay = 110f;
        timeNormalized = timeOfDay / dayLengthInSeconds;

        float curveValue = sunCurve.Evaluate(timeNormalized);
        float angle = curveValue * 360f;
        sunLight.transform.rotation = Quaternion.Euler(angle, 0f, 0f);

        UpdateLighting();
    }
}