using Unity.VisualScripting;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Camera mainCamera;
    private float CameraZDistance;
    private float mZCoord;
    public GameObject Toilet;
    public bool unClogged = false;
    public int counter = 0;

    private void Start()
    {
        mainCamera = Camera.main;
        CameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z;
    }


    private void OnMouseDrag()
    {
        Vector3 ScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance);
        Vector3 NewWorldPosition = mainCamera.ScreenToWorldPoint(ScreenPosition);
        transform.position = NewWorldPosition;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
