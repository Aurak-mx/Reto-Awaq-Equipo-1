using UnityEditor.Build;
using UnityEngine;

public class BossProjectileBehavior : MonoBehaviour
{
    public float speed = 5f; // Velocidad proyectil
    public float lifetime = 4f; // Tiempo de vida máximo proyectil 
    private Vector2 targetDirection; // Ruta para llegar a jugador

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Proyectil se destruye si termina el tiempo de "lifetime"
        Destroy(gameObject, lifetime); 

        // Búscamos y guardamos a el jugador 
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Destino "Player" menos el origen "Proyectil" == Dirección en vector 2d (X, Y)
            targetDirection = (player.transform.position - transform.position).normalized;

            //⭐️Rotamos sprite de semilla para que apunte hacia donde va
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg; 
            // Quaternion.Euler traduce nuestros grados humanos al sistema matematico complejo de Unity.
            // Lo aplicamos en el eje Z (0, 0, angle) porque en 2D la rotacion ocurre como las manecillas de un reloj.
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));  
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(targetDirection * speed * Time.deltaTime, Space.World); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("PUM! El Boss le dio a Player"); 

            // Buscamos script "PlayerControl" dentro de jugador con el que chocamos
            PlayerControl playerScript = collision.GetComponent<PlayerControl>(); 

            if (playerScript != null) // Validamos que jugador tenga script
            {
                playerScript.TakeDamage(); // Jugador toma daño 
            }
            
            Destroy(gameObject); 
        }
    }
}
