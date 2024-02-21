using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    protected GameObject cam;
    [SerializeField] protected float parallaxEffect;

    private float length;
    private float startPos;
    private float offset;

    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        offset = transform.position.x - cam.transform.position.x;

        Application.targetFrameRate = 120;
    }

    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - parallaxEffect);
        float dist = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length)
        {
            startPos = startPos + length + offset;
        }
    }
}
