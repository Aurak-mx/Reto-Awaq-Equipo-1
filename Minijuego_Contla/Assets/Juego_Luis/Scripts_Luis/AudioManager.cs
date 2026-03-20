using UnityEngine;

// Maneja todos los sonidos del juego desde un solo lugar
// Otros scripts llaman los metodos de esta clase para reproducir audio
public class AudioManager : MonoBehaviour
{
    // Instancia global para acceder desde cualquier script sin necesitar referencia directa
    public static AudioManager instance;

    [Header("Musica de Fondo")]
    public AudioClip musicaFondo;
    [Range(0f, 1f)] public float volumenFondo = 0.5f;

    [Header("Sonidos de Preguntas")]
    public AudioClip sonidoPregunta;
    [Range(0f, 1f)] public float volumenPregunta = 1f;
    public AudioClip sonidoCorrecto;
    [Range(0f, 1f)] public float volumenCorrecto = 1f;
    public AudioClip sonidoIncorrecto;
    [Range(0f, 1f)] public float volumenIncorrecto = 1f;

    [Header("Sonidos de Peligro")]
    public AudioClip sonidoPeligro;
    [Range(0f, 1f)] public float volumenPeligro = 1f;
    public AudioClip sonidoCocodrilo;
    [Range(0f, 1f)] public float volumenCocodrilo = 1f;
    public AudioClip sonidoPajaro;
    [Range(0f, 1f)] public float volumenPajaro = 1f;

    [Header("Sonidos de Resultado")]
    public AudioClip sonidoGanar;
    [Range(0f, 1f)] public float volumenGanar = 1f;
    public AudioClip sonidoPerder;
    [Range(0f, 1f)] public float volumenPerder = 1f;

    [Header("Sonidos de UI")]
    public AudioClip sonidoBoton;
    [Range(0f, 1f)] public float volumenBoton = 1f;

    // Fuentes de audio separadas para musica y efectos
    private AudioSource musicaSource;  // Para el loop de musica de fondo
    private AudioSource efectosSource; // Para todos los efectos de sonido

    void Awake()
    {
        // Patron Singleton: solo existe una instancia del AudioManager en toda la escena
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Crea dos AudioSources en el mismo objeto: uno para musica, otro para efectos
        musicaSource = gameObject.AddComponent<AudioSource>();
        efectosSource = gameObject.AddComponent<AudioSource>();

        // Configura la musica de fondo para que sea loop y empiece automaticamente
        musicaSource.clip = musicaFondo;
        musicaSource.loop = true;
        musicaSource.playOnAwake = true;
        musicaSource.volume = volumenFondo;
        musicaSource.Play();

        // Los efectos no necesitan loop
        efectosSource.loop = false;
        efectosSource.playOnAwake = false;
    }

    // --- Metodos publicos que llaman los demas scripts ---

    // Sonido al mostrar una nueva pregunta
    public void PlayPregunta()
    {
        PlayEfecto(sonidoPregunta, volumenPregunta);
    }

    // Sonido al responder correctamente
    public void PlayCorrecto()
    {
        PlayEfecto(sonidoCorrecto, volumenCorrecto);
    }

    // Sonido al responder incorrectamente
    public void PlayIncorrecto()
    {
        PlayEfecto(sonidoIncorrecto, volumenIncorrecto);
    }

    // Sonido cuando aparece el panel de peligro
    public void PlayPeligro()
    {
        PlayEfecto(sonidoPeligro, volumenPeligro);
    }

    // Sonido cuando el cocodrilo empieza a saltar
    public void PlayCocodrilo()
    {
        PlayEfecto(sonidoCocodrilo, volumenCocodrilo);
    }

    // Sonido cuando el pajaro entra en pantalla
    public void PlayPajaro()
    {
        PlayEfecto(sonidoPajaro, volumenPajaro);
    }

    // Sonido al ganar el juego, detiene la musica de fondo
    public void PlayGanar()
    {
        musicaSource.Stop();
        PlayEfecto(sonidoGanar, volumenGanar);
    }

    // Sonido al perder el juego, detiene la musica de fondo
    public void PlayPerder()
    {
        musicaSource.Stop();
        PlayEfecto(sonidoPerder, volumenPerder);
    }

    // Sonido al presionar cualquier boton de UI
    public void PlayBoton()
    {
        PlayEfecto(sonidoBoton, volumenBoton);
    }

    // Metodo interno que reproduce un clip con su volumen especifico
    // PlayOneShot permite que varios sonidos se superpongan sin cortarse
    private void PlayEfecto(AudioClip clip, float volumen)
    {
        if (clip == null) return; // Evita errores si el clip no esta asignado en el Inspector
        efectosSource.PlayOneShot(clip, volumen);
    }
}