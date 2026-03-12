using UnityEngine;

public class BossBehavior : MonoBehaviour
{

    public GameObject enemyBulletPrefab; // Prefab de semilla a disparar
    public Transform firePoint; // El punto del cual saldrá la semilla 
    public float minShootDelay = 2f; // Tiempo mínimo para disparar
    public float maxShootDelay = 5f; // Tiempo máximo para disparar
    private float shootTimer; // Cronómetro interno
    private Animator anim; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>(); 

        shootTimer = Random.Range(minShootDelay, maxShootDelay); 
        
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime; 

        if (shootTimer <= 0f)
        {
            StartAttack(); 

            shootTimer = Random.Range(minShootDelay, maxShootDelay); 
        }
        
    }

    void StartAttack()
    {
        // Prendemos animación de "isAttacking" para el boss
        anim.SetBool("isAttacking", true); 

        // Dispara la bala con delay ( para que coincida con animación )
        Invoke("Shoot", 0.3f);

        // Apagamos la animación 0.15 segundos después de que inicie a disparar
        Invoke("StopAttacking", 0.45f);  
    }

    void Shoot()
    {

        GameControl.Instance.sfxManager.EnemyThrowSound(); // Sonido de Diaparar de Boss
        // Creamos bala de boss
        Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity); 
    }

    void StopAttacking()
    {
        anim.SetBool("isAttacking", false); 
    }
}
