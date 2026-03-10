using UnityEngine;

public class PlatformDown : MonoBehaviour
{

    public float scrollSpeed = 2.5f; // this variable determines how quick an object moves ( in this case descends -2.5f per second )

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * scrollSpeed * Time.deltaTime); 

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
