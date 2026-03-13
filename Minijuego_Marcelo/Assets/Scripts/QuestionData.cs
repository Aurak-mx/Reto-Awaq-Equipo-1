// Este archivo sirve como base para las pregunta que se van a poner en el canvas
// dentro del juego. 

[System.Serializable] // Permite que unity interprete los datos de la clase
public class QuestionData
{
    public string questionText; // Texto de pregunta principal
    public string leftAnswerText; // Texto de opción respuesta a la izquierda
    public string rightAnswertext; // Texto de opción respuesta a la rececha
    public int correctButtonIndex; // 1 -> Izquierdo es correcto | 2 -> Derecho es correcto

    public bool isAnsweredCorrectly; // Identifica si pregunta ya fue contestada correctamente o no
}