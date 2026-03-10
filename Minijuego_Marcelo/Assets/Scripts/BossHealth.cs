using UnityEngine;
using UnityEngine.UI; // Necesario para cotrolar la barra de vida

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 2; 
    private int currentHealth; 
    public Image healthBarFill; 

    private Animator anim; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth; 

        // Buscamos el Animator en el objeto de la planta
        anim = GetComponent<Animator>(); 

        // Inicializamos la barra visual
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = 1f; 
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; 

        // Actualizamos barra de vida de planta que tomo "golpe"
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth; 
        }

        if (anim != null) // Si encontramos el componente de animaciones para la planta, ponemos animación "isHurt"
        {
            anim.SetBool("isHurt", true); 
            Invoke("StopHurt", 0.25f); 
        }

        Debug.Log(gameObject.name + " recibió daño. Vida: " + currentHealth); 

        if (currentHealth <= 0)
        {
            Die(); 
        }
    }

    void StopHurt()
    {
        if (anim != null)
        {
            anim.SetBool("isHurt", false); 
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " ha muerto."); 

        // Obtener todos los objetos en escena con tag de "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); 

        if (enemies.Length <= 1)
        {
            Debug.Log("Ganaste! Todas las plantas fueron eliminadas!"); 

            // Buscamos UIController y mandamos datos de "Victoria", 500 XP, y Medalla de "Oro"
            FindObjectOfType<UIController>().ShowEndGameScreen(true, 500, "Oro"); 
        }

        Destroy(gameObject); // Destruir planta que perdio sus vidas
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
