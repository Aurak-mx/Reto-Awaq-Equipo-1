using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab de "projectile"
    public Transform firePoint; // Punto invisible de donde saldra la bala
    public float moveSpeed; 
    public float jumpForce; 
    public Rigidbody2D rig; 
    private float xInput; 
    private bool jumpRequested; 

    Animator animatorController; 
    private bool isGrounded = true; 

    public SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animatorController = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        xInput = 0f; 
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            xInput = -1f; // Movimiento a la izquierda
        
        }
        else if (Keyboard.current.rightArrowKey.isPressed)
        {
            xInput = 1f; // Movimiento a la derecha
        }

        if (Keyboard.current.upArrowKey.wasPressedThisFrame && isGrounded) // Salto solo si el jugador está en el suelo
        {
            jumpRequested = true;
            isGrounded = false; 
        }

        if (xInput !=0 )
        {
            sr.flipX = xInput <0;  // Cambia la dirección de nina si se mueve a la izquierda. 
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (GameControl.Instance.GetCurrentAmmo() > 0)
            {
                // Llamamos spend ammo para que se deduzca en el UIController & GameController
                GameControl.Instance.SpendAmmo(); 

                // Hacemos animación y disparamos bola de nieve
                Shoot(); 
            }
            else
            {
                Debug.Log("¡Sin munición!"); 
            }
        }

        UpdatePlayerAnimation();
    }

    public void FixedUpdate()
    {
        rig.linearVelocity = new Vector2(xInput * moveSpeed, rig.linearVelocity.y); // Movimiento horizontal basado en la entrada del jugador

        if (jumpRequested) // Si se solicito un salto, se aplica fuerza de salto y se reinicia la variable de salto solicitado
        {
            GameControl.Instance.sfxManager.JumpSound(); // Sonido al brincar
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); 
            jumpRequested = false; 
        }

        if (Mathf.Abs(rig.linearVelocity.y) < 0.01f) // Si velocidad vertical es casi cero, se considera que jugador esta en el suelo
        {
            isGrounded = true; 
        }
    }

    public enum PlayerAnimation
    {
        idle, walk, jump
    }

    // Esta función se llama para actualizar la animación del jugador según el estado actual (idle, walk, jump)
    void UpdateAnimation(PlayerAnimation nameAnimation)
    {
        switch(nameAnimation)
        {
            case PlayerAnimation.idle:
                animatorController.SetBool("isWalking", false); 
                animatorController.SetBool("isJumping", false); 
                break;
            case PlayerAnimation.walk:
                animatorController.SetBool("isWalking", true);
                animatorController.SetBool("isJumping", false);
                break;
            case PlayerAnimation.jump:
                animatorController.SetBool("isWalking", false);
                animatorController.SetBool("isJumping", true);
                break;
        }
    }

    // Esta función se llama para determinar qué animación debe reproducirse según el estado del jugador ( en el suelo, caminando, saltando )
    void UpdatePlayerAnimation()
    {
        if (!isGrounded) // Animación de salto si jugador no está en el suelo
        {
            UpdateAnimation(PlayerAnimation.jump); 
        }
        else if (xInput != 0f) // Animación de caminar si jugador se esta moviendo horizontalmente y esta en el suelo
        {
            UpdateAnimation(PlayerAnimation.walk); 
        }
        else // Animación de idle si jugador no se esta moviendo horizontalmente y esta en el suelo
        {
            UpdateAnimation(PlayerAnimation.idle); 
        }
    }


    void Shoot()
    {
        // Prendemos switch de la animación de lanzar
        animatorController.SetBool("isFighting", true); 

        GameControl.Instance.sfxManager.ThrowSound(); 

        // Llamamos función SpawnProjectile para crear proyectil con un retraso de .15 segundos
        Invoke("SpawnProjectile", 0.18f); 

        // Llamamos función StopFighting .3 segundos después de iniciar para que animación pare. 
        Invoke("StopFighting", 0.3f); 
    }

    void SpawnProjectile()
    {
        // Se crea bala en posición de "firePoint"
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity); 

        // Izquierda (-1) --> flipX = true | Derecha (1) --> flipX = false
        float shootDirection = sr.flipX ? -1f : 1f; 

        bullet.GetComponent<ProjectileBehavior>().SetDirection(shootDirection); 
    }

    // Función que "Invoke" llama para apagar animación
    void StopFighting()
    {
        animatorController.SetBool("isFighting", false); 
    }

    // Función llamada por "semilla enemigo" cuando choca con "jugador"
    public void TakeDamage()
    {
        // Activamos animación de "hurt" (Acción temporal)
        animatorController.SetBool("isHurt", true); 

        // Apagamos animación de "hurt"
        Invoke("StopHurt", 0.3f); 

        // Sonido de que le pegaron a el jugador
        GameControl.Instance.sfxManager.PlayerHitSound(); 

        // Avisamos a GameControl para que quite vida y actualice UI
        GameControl.Instance.SpendLives(); 
    }

    void StopHurt()
    {
        animatorController.SetBool("isHurt", false); 
    }
}
