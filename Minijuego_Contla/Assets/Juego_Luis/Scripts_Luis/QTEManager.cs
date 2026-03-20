using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Maneja el evento de reaccion rapida (QTE) cuando aparece un animal
// El jugador debe presionar ESPACIO varias veces en poco tiempo para esquivar
public class QTEManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject qtePanel;             // Panel de peligro que se muestra durante el QTE
    public TextMeshProUGUI dangerText;      // Texto de "PELIGRO" o similar
    public TextMeshProUGUI instructionText; // Texto que indica cuantas veces presionar espacio
    public Image qteTimerBar;              // Barra que muestra el tiempo restante del QTE

    [Header("Configuracion")]
    public float qteTime = 1.5f;    // Segundos que tiene el jugador para completar el QTE
    public int requiredPresses = 3; // Cuantas veces debe presionar ESPACIO para esquivar

    [Header("Referencias")]
    public QuestionManager questionManager; // Para mostrar game over si falla en el eslabon mas bajo
    public ChainGrabSystem chainSystem;     // Para aplicar el balanceo al esquivar
    public AnimalSpawner animalSpawner;     // Para notificar el resultado del QTE

    private int currentPresses = 0; // Contador de cuantas veces ha presionado espacio
    private float timer = 0f;       // Tiempo restante del QTE
    private bool qteActive = false; // Si el QTE esta activo o no

    void Start()
    {
        qtePanel.SetActive(false);
    }

    void Update()
    {
        if (!qteActive) return;

        timer -= Time.deltaTime;
        qteTimerBar.fillAmount = timer / qteTime;
        instructionText.text = "      Presiona ESPACIO " + (requiredPresses - currentPresses) + " veces";

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentPresses++;
            if (currentPresses >= requiredPresses)
                QTESuccess();
        }

        if (timer <= 0 && qteActive)
            QTEFail();
    }

    // Inicia el QTE, se llama desde AnimalSpawner
    public void StartQTE()
    {
        currentPresses = 0;
        timer = qteTime;
        qteActive = true;
        qtePanel.SetActive(true);

        AudioManager.instance.PlayPeligro(); // Sonido cuando aparece el panel de peligro
    }

    // El jugador esquivo exitosamente
    void QTESuccess()
    {
        qteActive = false;
        qtePanel.SetActive(false);
        chainSystem.DodgeSwing(Random.value > 0.5f ? 1f : -1f);
        animalSpawner.OnQTEFinished(true);
    }

    // El jugador no completo el QTE a tiempo
    void QTEFail()
    {
        qteActive = false;
        qtePanel.SetActive(false);
        AudioManager.instance.PlayIncorrecto(); // Sonido de incorrecto al fallar el QTE
        animalSpawner.OnQTEFinished(false);
    }
   
}