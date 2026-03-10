using UnityEngine;
using UnityEngine.AI;

public class ChestBehavior : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0f; // Congelamos el juego si cofre colisiona con un Player

            if (GameControl.Instance != null && GameControl.Instance.uiController != null) // Llamamos función de UIController para "OpenQuestionPanel"
            {
                GameControl.Instance.uiController.OpenQuestionPanel(); 
            }

            gameObject.SetActive(false); // apagamos el cofre
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
