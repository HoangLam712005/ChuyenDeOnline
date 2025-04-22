using UnityEngine;

public class RotateAvatar_M3 : MonoBehaviour
{
    public float speed = 25f;
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}