using UnityEngine;
using System.Collections;

// Controla el movimiento del cocodrilo cuando aparece como evento de peligro
// El cocodrilo siempre sube desde el centro de la pantalla hacia la posicion del jugador
public class CrocodileMovement : MonoBehaviour
{
    public float jumpSpeed = 5f;  // Velocidad a la que sube y baja el cocodrilo
    public float waitAtTop = 1f;  // Segundos que se queda en la posicion mas alta

    private float targetY; // Posicion Y a la que va a llegar el cocodrilo
    private float startY;  // Posicion Y desde donde empieza a subir

    // Posiciona y lanza al cocodrilo, regresa la duracion total de la animacion
    public float Launch(float playerY)
    {
        transform.position = new Vector3(0f, playerY - 5f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 90);

        targetY = playerY + 2f;
        startY = transform.position.y;

        AudioManager.instance.PlayCocodrilo(); // Sonido cuando el cocodrilo empieza a saltar

        StartCoroutine(CrocodileJump());

        float distance = targetY - startY;
        return (distance / jumpSpeed) * 2 + waitAtTop;
    }

    // Animacion del salto: sube, espera, gira y baja
    IEnumerator CrocodileJump()
    {
        // Fase de subida
        while (transform.position.y < targetY)
        {
            transform.position += Vector3.up * jumpSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(waitAtTop);

        // Cambia rotacion para apuntar hacia abajo al bajar
        transform.rotation = Quaternion.Euler(0, 0, -90);

        // Fase de bajada
        while (transform.position.y > startY)
        {
            transform.position -= Vector3.up * jumpSpeed * Time.deltaTime;
            yield return null;
        }
    }
}