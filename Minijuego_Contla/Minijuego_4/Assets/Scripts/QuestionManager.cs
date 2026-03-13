using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// Maneja toda la logica de preguntas, respuestas, y los paneles de ganar/perder
public class QuestionManager : MonoBehaviour
{
    [Header("Panel Preguntas")]
    public GameObject questionCanvas;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    public Image timerBar;
    public GameObject button1Object;
    public GameObject button2Object;

    [Header("Panel GameOver")]
    public GameObject gameOverPanel;
    public GameObject gameOverButton1; // Boton reintentar
    public GameObject gameOverButton2; // Boton menu

    [Header("Panel Win")]
    public GameObject winPanel;
    public GameObject winButton1;      // Boton siguiente nivel
    public GameObject winButton2;      // Boton menu
    public TextMeshProUGUI totemText;
    public GameObject medalOro;
    public GameObject medalPlata;
    public GameObject medalBronce;

    [Header("Configuracion")]
    public float timeLimit = 10f;
    public float delayToShow = 2f;

    [Header("Referencias")]
    public ChainGrabSystem chainSystem;
    public GameTimer gameTimer;

    private List<Question> questionBank = new List<Question>();
    private Question currentQuestion;
    private bool correctIsButton1;
    private float timer;
    private bool timerRunning = false;
    private bool gameEnded = false;
    private bool questionActive = false;

    void Start()
    {
        LoadQuestions();
        questionCanvas.SetActive(false);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        Invoke("ShowQuestion", delayToShow);
    }

    void Update()
    {
        if (!timerRunning) return;

        timer -= Time.deltaTime;
        timerBar.fillAmount = timer / timeLimit;

        if (timer <= 0)
        {
            timerRunning = false;
            OnAnswer(false);
        }
    }

    public void ShowGameOver()
    {
        if (gameEnded) return;
        gameEnded = true;
        gameTimer.StopTimer();
        CancelInvoke();
        questionCanvas.SetActive(false);

        AudioManager.instance.PlayPerder(); // Sonido de derrota

        gameOverPanel.SetActive(true);

        gameOverButton1.GetComponent<Button>().onClick.RemoveAllListeners();
        gameOverButton2.GetComponent<Button>().onClick.RemoveAllListeners();
        gameOverButton1.GetComponent<Button>().onClick.AddListener(RetryLevel);
        gameOverButton2.GetComponent<Button>().onClick.AddListener(GoToMenu);
    }

    public void ShowWin()
    {
        if (gameEnded) return;
        gameEnded = true;
        gameTimer.StopTimer();
        CancelInvoke();
        questionCanvas.SetActive(false);

        AudioManager.instance.PlayGanar(); // Sonido de victoria

        float timeRemaining = gameTimer.GetCurrentTime();
        medalOro.SetActive(false);
        medalPlata.SetActive(false);
        medalBronce.SetActive(false);

        if (timeRemaining >= 120f)
        {
            medalOro.SetActive(true);
            totemText.text = "1000 Totems";
        }
        else if (timeRemaining >= 60f)
        {
            medalPlata.SetActive(true);
            totemText.text = "750 Totems";
        }
        else
        {
            medalBronce.SetActive(true);
            totemText.text = "500 Totems";
        }

        winPanel.SetActive(true);

        winButton1.GetComponent<Button>().onClick.RemoveAllListeners();
        winButton2.GetComponent<Button>().onClick.RemoveAllListeners();
        winButton1.GetComponent<Button>().onClick.AddListener(NextLevel);
        winButton2.GetComponent<Button>().onClick.AddListener(GoToMenu);
    }

    void GoToMenu()
    {
        AudioManager.instance.PlayBoton();
        Time.timeScale = 1f;
        // SceneManager.LoadScene("MenuScene");
        Debug.Log("Volver al Menu");
    }

    void NextLevel()
    {
        AudioManager.instance.PlayBoton();
        Time.timeScale = 1f;
        // SceneManager.LoadScene("SiguienteEscena");
        Debug.Log("Siguiente Nivel");
    }

    void RetryLevel()
    {
        AudioManager.instance.PlayBoton();
        Time.timeScale = 1f;
        Invoke("CargarEscena", 0.2f); // Espera 0.2s para que suene el boton
    }

    void CargarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void LoadQuestions()
    {
        questionBank.Add(new Question { questionText = "¿Que anota un pasante?", correctAnswer = "Datos", wrongAnswer = "Chistes" });
        questionBank.Add(new Question { questionText = "¿Que se monitorea en campo?", correctAnswer = "Fauna", wrongAnswer = "Semaforos" });
        questionBank.Add(new Question { questionText = "¿Que protege una reserva?", correctAnswer = "Habitat", wrongAnswer = "Asfalto" });
        questionBank.Add(new Question { questionText = "¿Que usa un biologo en campo?", correctAnswer = "Bitacora", wrongAnswer = "Control" });
        questionBank.Add(new Question { questionText = "¿Que necesita una muestra?", correctAnswer = "Etiqueta", wrongAnswer = "Brillo" });
        questionBank.Add(new Question { questionText = "¿Que estudia la ecologia?", correctAnswer = "Relaciones", wrongAnswer = "Monedas" });
        questionBank.Add(new Question { questionText = "¿Que ayuda a conservar?", correctAnswer = "Cuidado", wrongAnswer = "Ruido" });
        questionBank.Add(new Question { questionText = "¿Que se identifica en campo?", correctAnswer = "Especies", wrongAnswer = "Marcas" });
        questionBank.Add(new Question { questionText = "¿Que se evita en reserva?", correctAnswer = "Basura", wrongAnswer = "Sombra" });
        questionBank.Add(new Question { questionText = "¿Que protege AWAQ?", correctAnswer = "Naturaleza", wrongAnswer = "Concreto" });
        questionBank.Add(new Question { questionText = "¿Que mide un termometro?", correctAnswer = "Temperatura", wrongAnswer = "Altura" });
        questionBank.Add(new Question { questionText = "¿Que indica una huella?", correctAnswer = "Presencia", wrongAnswer = "Clima" });
        questionBank.Add(new Question { questionText = "¿Que produce una planta?", correctAnswer = "Oxigeno", wrongAnswer = "Humo" });
        questionBank.Add(new Question { questionText = "¿Que afecta un incendio?", correctAnswer = "Habitat", wrongAnswer = "Marea" });
        questionBank.Add(new Question { questionText = "¿Que contamina un rio?", correctAnswer = "Desechos", wrongAnswer = "Piedras" });
        questionBank.Add(new Question { questionText = "¿Que protege el suelo?", correctAnswer = "Raices", wrongAnswer = "Vidrio" });
        questionBank.Add(new Question { questionText = "¿Que requiere el muestreo?", correctAnswer = "Orden", wrongAnswer = "Velocidad" });
        questionBank.Add(new Question { questionText = "¿Que registra una camara trampa?", correctAnswer = "Fauna", wrongAnswer = "Nubes" });
        questionBank.Add(new Question { questionText = "¿Que animal es mamifero?", correctAnswer = "Jaguar", wrongAnswer = "Iguana" });
        questionBank.Add(new Question { questionText = "¿Que vive en humedales?", correctAnswer = "Anfibios", wrongAnswer = "Camellos" });
        questionBank.Add(new Question { questionText = "¿Que indica biodiversidad alta?", correctAnswer = "Variedad", wrongAnswer = "Sequedad" });
        questionBank.Add(new Question { questionText = "¿Que se revisa en monitoreo?", correctAnswer = "Cambios", wrongAnswer = "Ofertas" });
        questionBank.Add(new Question { questionText = "¿Que altera un ecosistema?", correctAnswer = "Contaminacion", wrongAnswer = "Neblina" });
        questionBank.Add(new Question { questionText = "¿Que ayuda a reforestar?", correctAnswer = "Nativas", wrongAnswer = "Plasticas" });
        questionBank.Add(new Question { questionText = "¿Que indica erosion?", correctAnswer = "Desgaste", wrongAnswer = "Sombra" });
        questionBank.Add(new Question { questionText = "¿Que busca conservar AWAQ?", correctAnswer = "Habitat", wrongAnswer = "Cemento" });
        questionBank.Add(new Question { questionText = "¿Que debe tener un registro?", correctAnswer = "Fecha", wrongAnswer = "Dibujo" });
        questionBank.Add(new Question { questionText = "¿Que revela una pluma?", correctAnswer = "Ave", wrongAnswer = "Reptil" });
        questionBank.Add(new Question { questionText = "¿Que grupo bioindica agua sana?", correctAnswer = "Anfibios", wrongAnswer = "Gatos" });
        questionBank.Add(new Question { questionText = "¿Que se cuida en una practica?", correctAnswer = "Protocolo", wrongAnswer = "Moda" });
        questionBank.Add(new Question { questionText = "¿Que especie llega primero tras disturbio?", correctAnswer = "Pionera", wrongAnswer = "Domestica" });
        questionBank.Add(new Question { questionText = "¿Que relacion beneficia a ambos?", correctAnswer = "Mutualismo", wrongAnswer = "Parasitismo" });
        questionBank.Add(new Question { questionText = "¿Que mide la humedad?", correctAnswer = "Higrometro", wrongAnswer = "Barometro" });
        questionBank.Add(new Question { questionText = "¿Que se evita al muestrear?", correctAnswer = "Sesgo", wrongAnswer = "Silencio" });
        questionBank.Add(new Question { questionText = "¿Que especie no es nativa?", correctAnswer = "Exotica", wrongAnswer = "Endemica" });
        questionBank.Add(new Question { questionText = "¿Que indica un bioindicador?", correctAnswer = "Calidad", wrongAnswer = "Velocidad" });
        questionBank.Add(new Question { questionText = "¿Que cadena inicia con plantas?", correctAnswer = "Trofica", wrongAnswer = "Metalica" });
        questionBank.Add(new Question { questionText = "¿Que organismo descompone materia?", correctAnswer = "Hongo", wrongAnswer = "Halcon" });
        questionBank.Add(new Question { questionText = "¿Que requiere una observacion valida?", correctAnswer = "Evidencia", wrongAnswer = "Suerte" });
        questionBank.Add(new Question { questionText = "¿Que protege mejor un habitat?", correctAnswer = "Restauracion", wrongAnswer = "Pavimento" });
    }

    public void ShowQuestion()
    {
        if (gameEnded) return;

        if (questionBank.Count == 0)
        {
            Debug.Log("Se acabaron las preguntas");
            return;
        }

        button1Object.SetActive(true);
        button2Object.SetActive(true);
        timerBar.gameObject.SetActive(true);
        questionText.fontSize = 27;

        button1Object.GetComponent<Button>().onClick.RemoveAllListeners();
        button2Object.GetComponent<Button>().onClick.RemoveAllListeners();
        button1Object.GetComponent<Button>().onClick.AddListener(OnButton1Click);
        button2Object.GetComponent<Button>().onClick.AddListener(OnButton2Click);

        int rand = Random.Range(0, questionBank.Count);
        currentQuestion = questionBank[rand];
        questionBank.RemoveAt(rand);

        correctIsButton1 = Random.value > 0.5f;
        questionText.text = currentQuestion.questionText;

        if (correctIsButton1)
        {
            button1Text.text = currentQuestion.correctAnswer;
            button2Text.text = currentQuestion.wrongAnswer;
        }
        else
        {
            button1Text.text = currentQuestion.wrongAnswer;
            button2Text.text = currentQuestion.correctAnswer;
        }

        questionCanvas.SetActive(true);
        timer = timeLimit;
        timerRunning = true;
        questionActive = true;

        AudioManager.instance.PlayPregunta(); // Sonido al mostrar nueva pregunta
    }

    public void OnButton1Click() { OnAnswer(correctIsButton1); }
    public void OnButton2Click() { OnAnswer(!correctIsButton1); }

    void OnAnswer(bool correct)
    {
        if (gameEnded) return;
        if (chainSystem.IsMoving()) return;

        questionActive = false;
        timerRunning = false;
        questionCanvas.SetActive(false);

        if (correct)
        {
            AudioManager.instance.PlayCorrecto(); // Sonido respuesta correcta
            chainSystem.MoveUp();
            Invoke("ShowQuestion", 2f);
        }
        else
        {
            AudioManager.instance.PlayIncorrecto(); // Sonido respuesta incorrecta
            if (chainSystem.IsAtBottom())
                ShowGameOver();
            else
            {
                chainSystem.MoveDown();
                Invoke("ShowQuestion", 2f);
            }
        }
    }

    public void PauseQuestion()
    {
        timerRunning = false;
        questionCanvas.SetActive(false);
    }

    public void ResumeQuestion()
    {
        if (!gameEnded)
        {
            questionCanvas.SetActive(true);
            timerRunning = true;
        }
    }

    public bool IsQuestionActive() { return questionActive; }
    public bool IsGameEnded() { return gameEnded; }
}