using UnityEngine;
using System.Collections;

// Maneja cuando y que animal aparece para interrumpir al jugador
// Dependiendo del eslabon donde este el jugador, aparece cocodrilo o pajaro
public class AnimalSpawner : MonoBehaviour
{
    [Header("Animales")]
    public GameObject crocodile; // Objeto del cocodrilo en la escena
    public GameObject bird;      // Objeto del pajaro en la escena

    [Header("Configuracion")]
    public float minTime = 1.5f; // Tiempo minimo entre apariciones de animales
    public float maxTime = 12f;  // Tiempo maximo entre apariciones de animales

    [Header("Referencias")]
    public ChainGrabSystem chainSystem;    // Para saber en que eslabon esta el jugador
    public QTEManager qteManager;          // Para iniciar el evento de reaccion rapida
    public QuestionManager questionManager; // Para pausar/reanudar la pregunta activa

    private GameObject currentAnimal;        // El animal que se va a lanzar en este evento
    private bool waitingForAnimation = false; // Evita que aparezcan dos animales al mismo tiempo
    private bool lastQTESuccess = false;      // Guarda si el jugador paso o fallo el QTE

    void Start()
    {
        StartCoroutine(SpawnTimer());
    }

    // Espera un tiempo aleatorio y luego intenta lanzar un animal
    // Usa WaitForSecondsRealtime para que funcione aunque el juego este pausado
    IEnumerator SpawnTimer()
    {
        while (true)
        {
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSecondsRealtime(waitTime);

            // Solo aparece un animal si hay una pregunta activa y no hay animacion en curso
            if (questionManager.IsQuestionActive() &&
                !waitingForAnimation &&
                !questionManager.IsGameEnded())
            {
                SpawnAnimal();
            }
        }
    }

    // Decide que animal aparece segun el eslabon actual del jugador
    void SpawnAnimal()
    {
        int index = chainSystem.GetCurrentIndex();

        // Eslabones bajos (0-3): aparece el cocodrilo desde abajo
        // Eslabon 4: zona neutral, no aparece nada
        // Eslabones altos (5-8): aparece el pajaro desde la izquierda
        if (index >= 0 && index <= 3)
            currentAnimal = crocodile;
        else if (index >= 5 && index <= 8)
            currentAnimal = bird;
        else
            return;

        // Pausa la pregunta y activa el panel de peligro
        questionManager.PauseQuestion();
        qteManager.StartQTE();
        StartCoroutine(LaunchAnimalAfterQTE());
    }

    // Espera a que termine el QTE y luego lanza el animal correspondiente
    IEnumerator LaunchAnimalAfterQTE()
    {
        // Guarda la posicion del jugador ANTES del QTE para que el animal apunte bien
        float playerY = chainSystem.transform.position.y;
        float playerX = chainSystem.transform.position.x;

        // Espera exactamente el tiempo del QTE antes de lanzar el animal
        yield return new WaitForSeconds(qteManager.qteTime);

        if (questionManager.IsGameEnded()) yield break;

        waitingForAnimation = true;
        currentAnimal.SetActive(true);

        if (currentAnimal == crocodile)
        {
            float duration = crocodile.GetComponent<CrocodileMovement>().Launch(playerY);

            // Si el jugador fallo el QTE, baja un eslabon 1.5 segundos despues de que empieza la animacion
            if (!lastQTESuccess)
            {
                yield return new WaitForSeconds(1.5f);
                if (chainSystem.IsAtBottom())
                    questionManager.ShowGameOver();
                else
                    chainSystem.MoveDown();
            }

            // Espera a que termine toda la animacion del cocodrilo
            yield return new WaitForSeconds(duration);
        }
        else if (currentAnimal == bird)
        {
            bird.GetComponent<BirdMovement>().Launch(playerY, playerX);

            // Igual que con el cocodrilo, el castigo llega 1.5 segundos despues
            if (!lastQTESuccess)
            {
                yield return new WaitForSeconds(1.5f);
                if (chainSystem.IsAtBottom())
                    questionManager.ShowGameOver();
                else
                    chainSystem.MoveDown();
            }

            // El pajaro se desactiva despues de 3 segundos
            yield return new WaitForSeconds(3f);
            bird.SetActive(false);
        }

        // Resetea la cadena y espera a que termine antes de reanudar la pregunta
        chainSystem.ResetChainPosition();
        yield return new WaitForSeconds(1.5f);

        waitingForAnimation = false;

        // Reactiva la pregunta que estaba pausada
        if (!questionManager.IsGameEnded())
            questionManager.ResumeQuestion();
    }

    // El QTEManager llama este metodo para informar si el jugador paso o fallo
    public void OnQTEFinished(bool success)
    {
        lastQTESuccess = success;
    }
}