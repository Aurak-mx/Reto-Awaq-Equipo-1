using UnityEngine;
using TMPro;

// Controla el temporizador general del juego
// Cuando llega a cero, el jugador pierde automaticamente
public class GameTimer : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI timerText; // Texto en pantalla que muestra el tiempo restante

    [Header("Configuracion")]
    public float totalTime = 180f; // Duracion total del juego en segundos (3 minutos)

    [Header("Referencia")]
    public QuestionManager questionManager; // Para avisar cuando se acabe el tiempo

    private float currentTime;        // Tiempo actual que va bajando
    private bool timerRunning = true; // Controla si el timer esta activo o no

    void Start()
    {
        // Inicializa el tiempo con el valor configurado en el Inspector
        currentTime = totalTime;
    }

    void Update()
    {
        if (!timerRunning) return;

        // Reduce el tiempo cada frame
        currentTime -= Time.deltaTime;

        // Si llego a cero, el jugador pierde
        if (currentTime <= 0)
        {
            currentTime = 0;
            timerRunning = false;
            questionManager.ShowGameOver();
        }

        // Formatea el tiempo como MM:SS y lo muestra en pantalla
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Detiene el timer, se llama cuando el juego termina (ganas o pierdes)
    public void StopTimer() { timerRunning = false; }

    // Regresa el tiempo actual, se usa para calcular la medalla al ganar
    public float GetCurrentTime() { return currentTime; }
}