using System.Collections;
using UnityEngine;

// Controla el movimiento del personaje en la cadena
// Maneja como se agarra, sube, baja, esquiva y resetea la cadena
public class ChainGrabSystem : MonoBehaviour
{
    [Header("Cadena")]
    public HingeJoint2D[] chainLinks;  // Los 10 eslabones de la cadena, de abajo hacia arriba
    public int startLinkIndex = 3;     // Eslabon donde el personaje empieza al caer
    public float dodgeForce = 15f;     // Fuerza del balanceo al esquivar un animal

    [Header("Referencias")]
    public Transform winTrigger;           // Posicion limite arriba, si la alcanza gana
    public QuestionManager questionManager; // Para avisar cuando llega arriba

    private Rigidbody2D rb;            // Componente fisico del personaje
    private DistanceJoint2D grabJoint; // Articulacion que une al personaje con el eslabon actual
    private int currentLinkIndex;      // Indice del eslabon donde esta parado actualmente
    private bool isGrabbed = false;    // Si el personaje esta agarrado a un eslabon
    private bool isMoving = false;     // Evita que se ejecuten dos movimientos al mismo tiempo
    private Animator animator;         // Para activar la animacion de subir/bajar

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;  // Evita que el personaje rote con la fisica
        rb.gravityScale = 2.3f;    // Cae con gravedad al inicio antes de agarrarse
        animator = GetComponent<Animator>();

        // Se agarra a la cadena despues de un pequeno delay para que se vea la caida
        Invoke("GrabChain", 0.77f);
    }

    // Engancha al personaje al eslabon de inicio
    void GrabChain()
    {
        currentLinkIndex = startLinkIndex;
        AttachToLink(currentLinkIndex);
    }

    // Crea una articulacion entre el personaje y el eslabon indicado
    void AttachToLink(int index)
    {
        isGrabbed = true;

        // Destruye la articulacion anterior si existe
        if (grabJoint != null)
            Destroy(grabJoint);

        // Detiene cualquier movimiento fisico 
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.freezeRotation = true;

        // Posiciona al personaje justo debajo del eslabon
        HingeJoint2D link = chainLinks[index];
        transform.position = link.transform.position + Vector3.down * 0.5f;

        // Crea la articulacion de distancia fija con el eslabon
        grabJoint = gameObject.AddComponent<DistanceJoint2D>();
        grabJoint.connectedBody = link.GetComponent<Rigidbody2D>();
        grabJoint.distance = 0.1f;
        grabJoint.autoConfigureDistance = false;
    }

    // Sube un eslabon si responde correctamente
    public void MoveUp()
    {
        if (isMoving) return;
        animator.SetTrigger("isClimbing");

        if (currentLinkIndex < chainLinks.Length - 1)
        {
            currentLinkIndex++;
            StartCoroutine(MoveToLink(currentLinkIndex));
        }
        else
        {
            // Si ya esta en el ultimo eslabon, sigue subiendo hasta el trigger de victoria
            StartCoroutine(KeepClimbing());
        }
    }

    // Animacion de subir libre hasta llegar al trigger de victoria
    IEnumerator KeepClimbing()
    {
        if (grabJoint != null) Destroy(grabJoint);
        isGrabbed = false;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        while (true)
        {
            transform.position += Vector3.up * 2f * Time.deltaTime;

            // Cuando alcanza la posicion del WinTrigger, termina el juego
            if (transform.position.y >= winTrigger.position.y)
            {
                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;
                StopAllCoroutines();
                questionManager.ShowWin();
                yield break;
            }

            yield return null;
        }
    }

    // Baja un eslabon si responde incorrectamente
    public void MoveDown()
    {
        if (isMoving) return;
        animator.SetTrigger("isClimbing");

        if (currentLinkIndex > 0)
        {
            currentLinkIndex--;
            StartCoroutine(MoveToLink(currentLinkIndex));
        }
    }

    // Mueve al personaje suavemente de un eslabon a otro
    IEnumerator MoveToLink(int index)
    {
        isMoving = true;

        // Frena todos los eslabones antes de moverse para que no haya vibracion rara
        for (int i = 0; i < chainLinks.Length; i++)
        {
            Rigidbody2D linkRb = chainLinks[i].GetComponent<Rigidbody2D>();
            linkRb.linearVelocity = Vector2.zero;
            linkRb.angularVelocity = 0f;
        }

        yield return new WaitForSeconds(0.5f);

        if (grabJoint != null) Destroy(grabJoint);

        float moveSpeed = 1.5f;

        // Se mueve hacia el eslabon destino, actualizando la posicion cada frame
        // por si el eslabon se esta moviendo por la fisica
        while (true)
        {
            Vector3 targetPos = chainLinks[index].transform.position + Vector3.down * 0.5f;

            if (Vector3.Distance(transform.position, targetPos) <= 0.005f)
                break;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        AttachToLink(index);
        isMoving = false;
    }

    // Aplica fuerza lateral a todos los eslabones para simular el esquive
    // La fuerza es mayor en los eslabones de abajo y menor en los de arriba
    public void DodgeSwing(float direction)
    {
        if (!isGrabbed) return;

        for (int i = 0; i < chainLinks.Length; i++)
        {
            Rigidbody2D linkRb = chainLinks[i].GetComponent<Rigidbody2D>();
            float forceMult = 1f - (float)i / chainLinks.Length;
            linkRb.AddForce(new Vector2(direction * dodgeForce * forceMult, 0), ForceMode2D.Impulse);
        }
    }

    // Inicia la rutina de reseteo suave de la cadena
    public void ResetChainPosition()
    {
        StartCoroutine(SmoothResetChain());
    }

    // Frena la cadena gradualmente con mucho drag, luego la regresa de golpe a su posicion original
    IEnumerator SmoothResetChain()
    {
        // Aumenta el drag para frenar el balanceo rapidamente
        for (int i = 0; i < chainLinks.Length; i++)
        {
            Rigidbody2D linkRb = chainLinks[i].GetComponent<Rigidbody2D>();
            linkRb.linearDamping = 5f;
            linkRb.angularDamping = 5f;
        }

        yield return new WaitForSeconds(2.5f);

        // Restaura el drag original y para cualquier movimiento residual
        for (int i = 0; i < chainLinks.Length; i++)
        {
            Rigidbody2D linkRb = chainLinks[i].GetComponent<Rigidbody2D>();
            linkRb.linearVelocity = Vector2.zero;
            linkRb.angularVelocity = 0f;
            linkRb.linearDamping = 0f;
            linkRb.angularDamping = 0.05f;
        }

        // Pequena pausa antes del snap para que se vea mas natural
        yield return new WaitForSeconds(0.5f);

        // Regresa todos los eslabones a su posicion X original de golpe
        for (int i = 0; i < chainLinks.Length; i++)
        {
            Rigidbody2D linkRb = chainLinks[i].GetComponent<Rigidbody2D>();
            Vector3 pos = chainLinks[i].transform.position;
            pos.x = 0.0965f; // Posicion X original de los eslabones en la escena
            chainLinks[i].transform.position = pos;
            linkRb.linearVelocity = Vector2.zero;
            linkRb.angularVelocity = 0f;
        }
    }

    // Metodos publicos para que otros scripts consulten el estado del personaje
    public bool IsAtBottom() { return currentLinkIndex == 0; }
    public bool IsAtTop() { return currentLinkIndex == chainLinks.Length - 1; }
    public int GetCurrentIndex() { return currentLinkIndex; }
    public bool IsMoving() { return isMoving; }
}