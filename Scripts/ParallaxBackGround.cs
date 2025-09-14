using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float parallaxEffect;
    private float xPosition;
    private float length;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > xPosition + length)
            xPosition += length;
        else if (distanceMoved < xPosition - length)
            xPosition -= length;

    }
}
