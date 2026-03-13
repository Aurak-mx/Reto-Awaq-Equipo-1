using System.Collections;
using System.Collections.Generic; // Necesario para filtrar preguntas pendientes
using JetBrains.Annotations;
using TMPro; // Necesario para editar los textos
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject questionPanel; // Game Object del panel que despliega preguntas
    public GameObject optionsPanel; // Game Object del panel que tiene opciones de juego
    public Sprite spendLives; 
    public Sprite spendAmmo; // Sprite que reemplaza a bola de nieve cuando se usa
    public Sprite fillAmmo; // Sprite de bola de nieve "llena"
    public Sprite medalPlatino; // Sprite medalla platino ( EASTER EGG )
    public Sprite medalGold; // Sprite medalla oro
    public Sprite medalSilver; // Sprite medalla silver
    public Sprite medalBronze; // Sprite medalla bronze
    public Image finalMedalImage; // Imagen en el panel final
    public Image[] livesImage; // Imágenes de vidas
    public Image[] ammoImages; // Imágenes de munición
    int lives = 3; 
    int ammo = 5; 

    // Variables para el XP (Tótems) component
    public TextMeshProUGUI xpText; 

    // Vínculos de componentes "TextMeshProUGUI" Unity a Backend con funcionalidad
    public TextMeshProUGUI questionUIText; 
    public TextMeshProUGUI leftButtonUIText; 
    public TextMeshProUGUI rightButtonUIText;
    public TextMeshProUGUI notificationText; // Mensaje estatus respuesta
    public GameObject notificationPanel; // Panel con mensaje de nofificación

    // Datos locales para creación y gestión de preguntas
    public QuestionData[] questionsList; 
    private int currentQuestionIndex = -1; 
    private float questionStartTime; 

    // Variables para panel "final de juego"
    public GameObject endGamePanel; 
    public TextMeshProUGUI endTitleText; 
    public TextMeshProUGUI endXpText; 
    public TextMeshProUGUI endMedalText; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lives = PlayerPrefs.GetInt("Lives"); 
    }


    public void UpdateLives()
    {
        lives = GameControl.Instance.GetCurrentLives(); 
        if (lives >= 0 && lives < livesImage.Length)
        {
            livesImage[lives].sprite = spendLives; 
        }
        GameControl.Instance.CheckGameOver(); 
    }

    public void UpdateAmmoDisplay()
    {
        ammo = GameControl.Instance.GetCurrentAmmo(); 

        for (int i = 0; i < ammoImages.Length; i++)
        {
            if (i < ammo)
            {
                ammoImages[i].sprite = fillAmmo; // Sprite de fillAmmo 
            }
            else
            {
                ammoImages[i].sprite = spendAmmo; // Sprite de spendAmmo
            }
        }
        
    }


    // Función se encarga de mostrar el question panel y la pregunta pertinente ( se usa al interactuar con un cofre )
    public void OpenQuestionPanel()
    {
        // Creamos lista temporal para preguntas que siguen "pendientes"
        List<int> pendingQuestions = GetPendingQuestions(); 

        if (pendingQuestions.Count > 0)
        {
            // Seleccionar pregunta aleatoria dentro de lista de preguntas posibles a preguntar
            int randomIndex = Random.Range(0, pendingQuestions.Count); 
            currentQuestionIndex = pendingQuestions[randomIndex]; 

            // Obtenemos información de pregunta actual
            QuestionData selectedQuestion = questionsList[currentQuestionIndex]; 

            // Modificamos texto de elementos textMeshProUGUI
            questionUIText.text = selectedQuestion.questionText; 
            leftButtonUIText.text = selectedQuestion.leftAnswerText; 
            rightButtonUIText.text = selectedQuestion.rightAnswertext; 

            questionPanel.SetActive(true);  // Activamos el panel de preguntas si tenemos preguntas pendientes
            questionStartTime = Time.realtimeSinceStartup; // Registra el segundo exacto en el cual se abrío el questions panel
        }
        else
        {
            // Caso especial para ganar, significa no matar a los jefes y responder TODAS las preguntas para este area. 
            Debug.Log("Felicidades! Ya respondiste todas las preguntas de Awaq Correctamente. "); 
            Time.timeScale = 1f; 
            ShowEndGameScreen(true, GameControl.Instance.currentXP, "Platino (Pacifista)"); 
        }
    }

    public void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true); // Abre el menú
        Time.timeScale = 0f; // Pausa el juego
    }

    public void SelectLeftBtn()
    {
        VerifyAnswer(1); 
    }

    public void SelectRightBtn()
    {
        VerifyAnswer(2); 
    }

    private void VerifyAnswer(int selectedOption)
    {
        float timeTaken = Time.realtimeSinceStartup - questionStartTime; // Diferencia de tiempo entre que se respondío la pregunta y que se abrío el panel. 
        bool correct;  
        

        // Si respuesta es correcta, marcar pregunta como "isAnsweredCorrectly" y poner correct = true
        if (questionsList[currentQuestionIndex].correctButtonIndex == selectedOption)
        {
            //GameControl.Instance.sfxManager.CorrectAnswerSound(); // Sonido de respuesta correcto (cofre)
            Debug.Log("RESPUESTA CORRECTA"); 
            questionsList[currentQuestionIndex].isAnsweredCorrectly = true; 
            correct = true; 

        }
        // Si respuesta es incorrecta, poner correct = false ( esto sirve para los tótems )
        else
        {
            //GameControl.Instance.sfxManager.IncorrectAnswerSound(); // Sonido de respuesta correcto (cofre)
            Debug.Log("RESPUESTA INCORRECTA");
            correct = false; 

        }

        // Calculamos los tótems ganados y agregamos a usuario
        int xpEarned = GameControl.Instance.AddCalculatedXP(timeTaken, correct); 

        if (correct)
        {
            GameControl.Instance.sfxManager.CorrectAnswerSound(); // Sonido de respuesta correcto (cofre)
            ShowNotificationText("¡CORRECTO! +" + xpEarned + " Tótems y +1 Bala", Color.green);
        }
        else
        {
            GameControl.Instance.sfxManager.IncorrectAnswerSound(); // Sonido de respuesta correcto (cofre)
            ShowNotificationText("¡INCORRECTO! +" + xpEarned + " Tótems de consolación", Color.red);
        }

        // Creamos lista temporal para preguntas que siguen "pendientes", despues de actualizar estado de pregunta contestada
        List<int> pendingQuestions = GetPendingQuestions();

        if (pendingQuestions.Count > 0)
        {
            CloseQuestionPanelAndResume(); 
        }

        else
        {
            // Caso especial para ganar, significa no matar a los jefes y responder TODAS las preguntas para este area. 
            Debug.Log("Felicidades! Ya respondiste todas las preguntas de Awaq Correctamente. "); 
            // Time.timeScale = 1f; 
            ShowEndGameScreen(true, GameControl.Instance.currentXP, "Platino (Pacifista)");
        }

        
    }

    public void CloseQuestionPanel() // Función se encarga de apagar el panel ( se usara al momento en el cuál el usuario conteste )
    {
        questionPanel.SetActive(false); 
    }

    public void SelectCorrectAnswer()
    {
        Debug.Log("Correct Answer Registered"); 
        CloseQuestionPanelAndResume(); 
    }

    private void CloseQuestionPanelAndResume()
    {
        if(questionPanel != null)
        {
            questionPanel.SetActive(false); 
        }

        Time.timeScale = 1f; 
    }

    public void ShowEndGameScreen(bool isWin, int xpGained, string medal)
    {

        // Desactivar Notifications Panel cuando termina juego
        if (notificationText != null)
        {
            notificationText.gameObject.SetActive(false);
        }
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false); 
        }

        // Pausamos juego 
        Time.timeScale = 0f; 

        // Definimos si gano o perdió
        if (isWin)
        {
            if (medal == "Platino (Pacifista)")
            {
                endTitleText.text = "¡VICTORIA PACIFISTA!"; 
                GameControl.Instance.sfxManager.PlayWinBGM(); // Música de ganar
            }
            else
            {
                endTitleText.text = "¡GANASTE!"; 
                GameControl.Instance.sfxManager.PlayWinBGM(); // Música de ganar
            }
            
        }
        else
        {
            endTitleText.text = "¡FIN DEL JUEGO!"; 
            GameControl.Instance.sfxManager.StopBGM(); // Parar de tocar música de fondo
            GameControl.Instance.sfxManager.LoseSound(); // Sonido de perder
        }

        // Adignamos valores de XP y Medalla
        endXpText.text = xpGained.ToString(); 

        // Funcionamiento despliegue de Medalla

        if (medal == "Oro")
        {
            finalMedalImage.sprite = medalGold; // Asignar a el final medal image la imágen de gold medal
            finalMedalImage.gameObject.SetActive(true); // Despliegar medalla
        }
        else if ( medal == "Plata")
        {
            finalMedalImage.sprite = medalSilver; // Asignar a el final medal image la imágen de silver medal
            finalMedalImage.gameObject.SetActive(true); // Despliegar medalla
        }
        else if ( medal == "Bronce")
        {
            finalMedalImage.sprite = medalBronze; // Asignar a el final medal image la imágen de bronze medal
            finalMedalImage.gameObject.SetActive(true); // Despliegar medalla
        }
        else if ( medal == "Platino (Pacifista)")
        {
            finalMedalImage.sprite = medalPlatino;
            finalMedalImage.gameObject.SetActive(true);
        }
        else
        {
            finalMedalImage.gameObject.SetActive(false); // Sin medalla
            endMedalText.text = "Medalla: " + "Ninguna"; 
        }

        

        // Activamos tableta de "endGamePanel"
        endGamePanel.SetActive(true); 
    }

    // Función para "Replay"
    public void ClickReplay()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    // Función para "Exit"
    public void ClickExit()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }

    public void ClickResume()
    {
        optionsPanel.SetActive(false); // Cierra el menú
        Time.timeScale = 1f; // Resume el juego
    }

    // Función para actualizar la XP Bar en el UI
    public void UpdateXPBar()
    {
        xpText.text = GameControl.Instance.currentXP + " / " + GameControl.Instance.xpForGold; 
    }

    public void ShowNotificationText(string message, Color msgColor)
    {

        notificationText.text = message; 
        notificationText.color = msgColor; 
        notificationText.gameObject.SetActive(true); 
        notificationPanel.SetActive(true); 

        Invoke("HideNotification", 2f); // Desactivamos componente "notificationText" después de 2 segundos
    }
    
    // Función para ocultar texto de notificación
    private void HideNotification()
    {
        notificationText.gameObject.SetActive(false); 
        notificationPanel.SetActive(false); 
    }

    public List<int> GetPendingQuestions()
    {
        // Creamos lista temporal para preguntas que siguen "pendientes"
        List<int> pendingQuestions = new List<int>(); 

       // Para cada pregunta en nuestra lista de preguntas
       for (int i = 0; i<questionsList.Length; i++)
        {
            // Para cada pregunta que no haya sido respondida correctamente
            if (questionsList[i].isAnsweredCorrectly == false) 
            {
                // Agregar pregunta a lista de "preguntas pendientes"
                pendingQuestions.Add(i); 
            }
        }

        return pendingQuestions; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
