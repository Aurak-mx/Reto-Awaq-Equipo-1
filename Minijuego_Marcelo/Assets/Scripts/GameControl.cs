using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{

    public int timeToWin = 15; 
    static public GameControl Instance; // Instancia de GameControl para usarse donde sea 
    public UIController uiController; 
    public int currentXP = 0; 
    public int xpForBronze = 600; 
    public int xpForSilver = 850; 
    public int xpForGold = 1200; 

    public SFXManager sfxManager; // Instancia de SFXManager para generar sonido en sistema

    // Función que corre el momento que inicia el juego
    public void Awake()
    {
        StopAllCoroutines(); 
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("Ammo", 5);
        // PlayerPrefs.SetInt("TimeToWin", PlayerPrefs.GetInt("TimeToWin", timeToWin)); 
        Instance = this; 
        Instance.SetReferences(); 
        DontDestroyOnLoad(this.gameObject); 
    }

    void SetReferences()
    {
        if (uiController == null)
        {
            uiController = FindAnyObjectByType<UIController>(); 
        }
        if (sfxManager == null)
        {
            sfxManager = FindAnyObjectByType<SFXManager>(); 
        }
        // timeToWin = PlayerPrefs.GetInt("TimeToWin"); 
        // init(); 
    
    }

    // void init()
    // {
    //     if(uiController != null)
    //     {
    //         uiController.StartTimer(); 
    //     }
    // }

    public int GetCurrentLives()
    {
        return PlayerPrefs.GetInt("Lives"); 
    }

    public int GetCurrentAmmo()
    {
        return PlayerPrefs.GetInt("Ammo"); 
    }

    public void SpendLives()
    {
        if (GetCurrentLives() > 0)
        {
            int newLives = GetCurrentLives() - 1;
            PlayerPrefs.SetInt("Lives", newLives); // PlayerPrefs
            uiController.UpdateLives();  
        }
        else
        {
            // El jugador perdió, mostrar tablet de derrota
            uiController.ShowEndGameScreen(false, currentXP, "Ninguna"); 
        }
        
    }

    // Función para reducir munición de jugador
    public void SpendAmmo()
    {
        if (GetCurrentAmmo() > 0)
        {
            int newAmmo = GetCurrentAmmo() - 1; 
            PlayerPrefs.SetInt("Ammo", newAmmo); 
            uiController.UpdateAmmoDisplay(); 
        }
        else
        {
            Debug.Log("NO TIENES BALAS RESTANTES"); 
        }
    }

    // Función ara incrementar munición de jugador
    public void AddAmmo(int amount)
    {
        int currentAmmo = GetCurrentAmmo(); 
        int maxAmmo = 5; 

        if (currentAmmo < maxAmmo)
        {
            int newAmmo = currentAmmo + amount; 

            if (newAmmo > maxAmmo)
            {
                newAmmo = maxAmmo; // Si obtenemos más ammo del máximo, topar en máximo
            }

            PlayerPrefs.SetInt("Ammo", newAmmo); 
            uiController.UpdateAmmoDisplay(); // Actualizamos Ammo
        }
    }

    public void CheckGameOver()
    {
        if(GetCurrentLives() == 0)
        {
            // El jugador perdió, mostrar tablet de derrota
            uiController.ShowEndGameScreen(false, currentXP, "Ninguna");
        }
    }

    // función para agregar "Tótems" 
    public void AddXP(int amount)
    {
        currentXP += amount; 

        uiController.UpdateXPBar(); // Actualizamos "Tótems" en Barra
    }

    public int AddCalculatedXP(float questionTime, bool isCorrect)
    {

        if (!isCorrect) // Si pregunta se respondío incorrectamente, dar solo 50 totems
        {
            AddXP(50); 
            return 50; 
        }

        // Si sacaron respuesta correcta, pueden obtener hasta 250 totems (dependiendo de tiempo de contestación)
        int baseScore = 250; 
        int penalty = 0; 

    
        if (questionTime > 20f) penalty = 100; // Si tardo más de 20 segundos, le restamos 100 tótems
        else if (questionTime > 10f) penalty = 50; // Si tardo más de 10 segundos pero MENOS de 20, quitar 50 tótems

        int finalScore = baseScore - penalty;
        AddXP(finalScore); // Agregar tótems finales correspondientes

        AddAmmo(1); // Agregar munición por responder correctamente

        return finalScore; 

    }

    public string GetMedal()
    {
        if (currentXP >= xpForGold)
        {
            return "Oro"; 
        }
        if (currentXP >= xpForSilver)
        {
            return "Plata"; 
        }
        if (currentXP >= xpForBronze)
        {
            return "Bronce"; 
        }

        return "Ninguna"; 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
