using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuManager : MonoBehaviour
{

    public GameObject mainMenuPanel; // Panel inicial ( Título & Acciones )
    public GameObject panelRules1; // Panel #1 para despliegar reglas de juego antes de comenzar
    public GameObject panelRules2; // Panel #2 para despliegar dínamica de juego antes de comenzar

    public void OpenRules1()
    {
        mainMenuPanel.SetActive(false); 
        panelRules1.SetActive(true); 
    }

    public void OpenRules2()
    {
        panelRules1.SetActive(false); 
        panelRules2.SetActive(true); 
        
    }

    public void CloseRules()
    {
        panelRules2.SetActive(false); 
        mainMenuPanel.SetActive(true); 
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game5"); 
        // panelRules2.SetActive(false); 
        // mainMenuPanel.SetActive(true); 
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
