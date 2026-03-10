using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    public float speed = 10f; // Velocidad de la bala
    public float direction = 1f; // 1 = derecha | -1 = izquierda
    public float lifetime = 3f; // Tiempo de vida de la bala ( para que no sea infinito )
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Destruye la bala si no interactua con nada en "lifetime" segundos
        Destroy(gameObject, lifetime); 
        
    }

    // Update is called once per frame
    void Update()
    {
        // Mueve la bala constantemente hacia la dirección en la cual se lanzo
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime); 
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("PUM! Le dimos al enemigo!");

            // Obtenemos script de vida para planta dañada
            BossHealth boss = collision.GetComponent<BossHealth>(); 

            // Si encontramos el script de vida del jefe, le quitamos uno de vida
            if (boss != null)
            {
                boss.TakeDamage(1); 
            }

            // La bala se estruye a sí misma depués de impacto
            Destroy(gameObject); 
                    
        }
    }

    public void SetDirection(float dir)
    {
        direction = dir; 

        if (dir < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true; 
        }
    }
}
