using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private float starpos;
    public float parallaxEffect;

    void Start()
    {
        starpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        float temp = Camera.main.transform.position.x * (1 - parallaxEffect);
        float dist = Camera.main.transform.position.x * parallaxEffect;
        transform.position = new Vector3(
            starpos + dist, transform.position.y, transform.position.z);

        if(temp > starpos + length)
            starpos += length;
        else if (temp < starpos - length)
            starpos -= length;
    }
}