using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip win; // Variable para elegir sonido ganar
    public AudioClip lose; // Variable para elegir sonido perder
    public AudioClip throwSound; // Variable para elegir sonido de throw
    public AudioClip enemyThrow; // Sonido de enemigo al disparar

    public AudioClip enemyHitSound; // Sonido cuando le damos un "hit" a una de las plantas
    public AudioClip playerHit; // Sonido de cuando se le da un "hit" al jugador
    public AudioClip playerJump; // Sonido de cuando jugador salta
    public AudioClip correctAnswer; // Sonido al sacar pregunta bien
    public AudioClip incorrectAnswer; // Sonido al sacar pregunta mal


    public AudioSource bgMusicSource; // AudioSource de nuestro SFXManager
    private AudioSource sfxSource; // Bocina interna 2D

    private void Awake()
    {
        sfxSource = gameObject.AddComponent<AudioSource>(); // Agregamos bocina a objeto

        //sfxSource.playOnAwake = false; // Instrucción: NO producir sonidor al azar al iniciar

        sfxSource.spatialBlend = 0f; // 0 significa sonido 100% 2D ( no importa posición orígen )
    }



    // Esta función llama a reproducir el sonido de los pájaros
    public void ThrowSound()
    {
        sfxSource.PlayOneShot(throwSound, 1f); 
    }

    public void JumpSound()
    {
        sfxSource.PlayOneShot(playerJump, 1f); 
    }

    public void EnemyThrowSound()
    {
        sfxSource.PlayOneShot(enemyThrow, 1f); 
    }

    public void EnemyHitSound()
    {
        sfxSource.PlayOneShot(enemyHitSound, 0.6f);
    }

    public void PlayerHitSound()
    {
        sfxSource.PlayOneShot(playerHit, 0.6f);
    }

    public void CorrectAnswerSound()
    {
        sfxSource.PlayOneShot(correctAnswer, 0.9f);
    }

    public void IncorrectAnswerSound()
    {
        sfxSource.PlayOneShot(incorrectAnswer, 0.9f);
    }

    // Esta función llama a reproducir el sonido de ganar
    public void WinSound() 
    {
        sfxSource.PlayOneShot(win, 0.5f);
    }

    // Esta función llama a reproducir el sonido de perder 
    public void LoseSound() 
    {
        sfxSource.PlayOneShot(lose, 0.5f);
    }

    public void PlayWinBGM()
    {
       bgMusicSource.Stop(); // Parar música de bg
       bgMusicSource.clip = win; // Cambiar la música de bg
       bgMusicSource.Play(); // Tocar música bg de win
    }

    public void StopBGM()
    {
        bgMusicSource.Stop(); 
    }


}



// using UnityEngine;

// public class SFXManager : MonoBehaviour
// {
//     public AudioClip win; // Variable para elegir sonido ganar
//     public AudioClip lose; // Variable para elegir sonido perder
//     public AudioClip throwSound; // Variable para elegir sonido de throw

//     public AudioClip enemyHitSound; // Sonido cuando le damos un "hit" a una de las plantas
//     public AudioClip correctAnswer; // Sonido al sacar pregunta bien
//     public AudioClip incorrectAnswer; // Sonido al sacar pregunta mal

//     private AudioSource sfxSource; // Bocina interna 2D

//     private void Awake()
//     {
//         sfxSource = gameObject.AddComponent<AudioSource>(); // Agregamos bocina a objeto

//         sfxSource.playOnAwake = false; // Instrucción: NO producir sonidor al azar al iniciar

//         sfxSource.spatialBlend = 0f; // 0 significa sonido 100% 2D ( no)
//     }



//     // Esta función llama a reproducir el sonido de los pájaros
//     public void ThrowSound()
//     {
//         AudioSource.PlayClipAtPoint(throwSound, Camera.main.transform.position, 1f); 
//     }

//     public void EnemyHitSound()
//     {
//         AudioSource.PlayClipAtPoint(enemyHitSound, Camera.main.transform.position, 0.6f);
//     }

//     public void CorrectAnswerSound()
//     {
//         AudioSource.PlayClipAtPoint(correctAnswer, Camera.main.transform.position, 0.9f);
//     }

//     public void IncorrectAnswerSound()
//     {
//         AudioSource.PlayClipAtPoint(incorrectAnswer, Camera.main.transform.position, 0.9f);
//     }

//     // Esta función llama a reproducir el sonido de ganar
//     public void WinSound() 
//     {
//         AudioSource.PlayClipAtPoint(win, Camera.main.transform.position, 0.5f);
//     }

//     // Esta función llama a reproducir el sonido de perder 
//     public void LoseSound() 
//     {
//         AudioSource.PlayClipAtPoint(lose, Camera.main.transform.position, 0.5f);
//     }

// }
