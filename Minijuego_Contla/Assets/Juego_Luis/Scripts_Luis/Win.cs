using UnityEngine;

// Trigger de victoria que detecta cuando el jugador llega hasta arriba
// Se activa con un pequeno delay al inicio para evitar detecciones falsas
public class WinTrigger : MonoBehaviour
{
    public QuestionManager questionManager; // Para llamar ShowWin cuando el jugador llega

    void Start()
    {
        // El collider empieza desactivado para evitar que se active antes de tiempo
        gameObject.SetActive(false);
        Invoke("Activate", 3f);
    }

    // Activa el collider del trigger despues del delay inicial
    void Activate()
    {
        gameObject.SetActive(true);
    }

    // Se detecta cuando el jugador entra al area del trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            questionManager.ShowWin();
        }
    }
}