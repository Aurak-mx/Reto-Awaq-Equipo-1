using UnityEngine;
using UnityEngine.SceneManagement;

// Maneja la pausa del juego
// Se activa con el boton de engranaje en la esquina superior derecha del HUD
public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;           // Panel con los botones de reanudar, reiniciar y menu
    public QuestionManager questionManager; // Para verificar que el juego no haya terminado

    private bool isPaused = false;

    // Alterna entre pausar y reanudar el juego
    // Se conecta al boton de engranaje en el Inspector
    public void TogglePause()
    {
        if (questionManager.IsGameEnded()) return;

        isPaused = !isPaused;

        if (isPaused)
        {
            AudioManager.instance.PlayBoton(); // Sonido al abrir el menu de pausa
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
        else
        {
            ResumeGame();
        }
    }

    // Reanuda el juego y oculta el menu de pausa
    public void ResumeGame()
    {
        AudioManager.instance.PlayBoton(); // Sonido al reanudar
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    // Reinicia la escena actual desde cero
    public void RestartGame()
    {
        AudioManager.instance.PlayBoton();
        Time.timeScale = 1f;
        Invoke("CargarEscena", 0.2f);
    }

    void CargarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Carga el menu principal
    public void GoToMenu()
    {
        AudioManager.instance.PlayBoton(); // Sonido al ir al menu
        Time.timeScale = 1f;
        // SceneManager.LoadScene("MenuScene");
        Debug.Log("Ir al Menu");
    }
}