using UnityEngine;

public class ChaserGael : MonoBehaviour
{
    public float moveSpeed = 2f;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;
    Animator animatorController;
    public float stopDistance = 0.1f;

    public SFXManagerGael sound;


    private void Awake()
    {
        rb= GetComponent<Rigidbody2D>();

    }

    public enum ChaserAnimation
    {
           idle, chase, 
    }

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animatorController = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            float distanceX = target.position.x - transform.position.x;
            if (Mathf.Abs(distanceX) > stopDistance)
            {
                moveDirection = new Vector2(Mathf.Sign(distanceX), 0);
            }
            else
            {
                moveDirection = Vector2.zero;
            }
        
        }

        UpdatePlayerAnimation();
    }

    void UpdatePlayerAnimation()
    {
        if (moveDirection.x != 0)
        {
            UpdateAnimation(ChaserAnimation.chase);
            if (moveDirection.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                UpdateAnimation(ChaserAnimation.idle);
            }
        }
    }

    void UpdateAnimation(ChaserAnimation nameAnimation)
    {
        switch (nameAnimation)
        {
            case ChaserAnimation.chase:
                animatorController.SetBool("isChasing", true);
                break;
            
            case ChaserAnimation.idle:
                animatorController.SetBool("isChasing", false);
                break;
                
        }
    }

    void FixedUpdate()
    {
       rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SpendLives();
                GameManager.Instance.SpendLives();
                GameManager.Instance.SpendLives();

                GameManager.Instance.sFXManager.ChompSound();

                Destroy(this.gameObject);
            }
        }
    }
    
}


