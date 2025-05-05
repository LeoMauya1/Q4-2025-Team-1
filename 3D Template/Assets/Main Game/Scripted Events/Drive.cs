using UnityEngine;

public class Drive : MonoBehaviour
{
    public Transform endPoint;
    public float driveSpeed;




    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPoint.position, driveSpeed *Time.deltaTime);
    }
}
