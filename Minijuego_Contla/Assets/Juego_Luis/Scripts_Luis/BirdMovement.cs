using UnityEngine;

// Controla el movimiento del pajaro cuando aparece como evento de peligro
// El pajaro entra desde la izquierda de la pantalla y vuela hacia donde estaba el jugador
public class BirdMovement : MonoBehaviour
{
    public float speed = 5f;

    private Vector3 targetDirection;
    private bool soundPlayed = false; // Evita que el sonido se repita multiples veces

    void OnEnable()
    {
        // Resetea el flag del sonido cada vez que el pajaro se activa
        soundPlayed = false;
        transform.position = new Vector3(-15f, transform.position.y, 0);
    }

    // Recibe la posicion del jugador y configura la trayectoria de vuelo
    public void Launch(float playerY, float playerX)
    {
        transform.position = new Vector3(-15f, playerY, 0);

        Vector3 target = new Vector3(playerX, playerY, 0);
        targetDirection = (target - transform.position).normalized;

        // Sonido cuando el pajaro aparece en pantalla
        if (!soundPlayed)
        {
            AudioManager.instance.PlayPajaro();
            soundPlayed = true;
        }
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        transform.position += targetDirection * speed * Time.deltaTime;

        if (transform.position.x >= 15f)
            gameObject.SetActive(false);
    }
}