// Clase que define la estructura de cada pregunta del juego
// Se usa [System.Serializable] para que Unity pueda mostrarla en el Inspector si se necesita
[System.Serializable]
public class Question
{
    public string questionText;   // El texto de la pregunta que se muestra en pantalla
    public string correctAnswer;  // La respuesta correcta
    public string wrongAnswer;    // La respuesta incorrecta (distractor)
}