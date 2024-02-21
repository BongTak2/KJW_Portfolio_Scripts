using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    [SerializeField] protected GameObject backGround;
    [SerializeField] protected GameObject endMap;

    private float offset;
    private Vector3 camPos;

    private void Start()
    {
        player = GameManager.instance.player;
        offset = transform.position.x - player.transform.position.x;
        camPos = transform.position;
    }

    void LateUpdate()
    {
        camPos.x = player.transform.position.x + offset;
        if (endMap != null)
        {
            if (endMap.activeSelf)
            {
                if (camPos.x >= endMap.transform.position.x)
                    camPos.x = endMap.transform.position.x;
            }
        }
        transform.position = camPos;
        backGround.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }
}
