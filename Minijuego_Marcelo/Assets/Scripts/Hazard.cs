using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos que lo que entro al trigger es el player
        if (other.CompareTag("Player"))
        {
            // Buscamos UIController en la escena
            UIController ui = Object.FindFirstObjectByType<UIController>(); 
            
            // Quitar todas las vidas del jugador
            GameControl.Instance.SpendLives(); 
            GameControl.Instance.SpendLives(); 
            GameControl.Instance.SpendLives(); 


            ui.ShowEndGameScreen(false, GameControl.Instance.currentXP, "Ninguna"); 
        }
    }

}
