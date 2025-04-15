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
    public GameObject cloth;
    private void Start()
    {
        mainCamera = Camera.main;
        CameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z;
        cloth.gameObject.SetActive(false);
    }


    private void OnMouseDrag()
    {
        Vector3 ScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance);
        Vector3 NewWorldPosition = mainCamera.ScreenToWorldPoint(ScreenPosition);
        transform.position = NewWorldPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().freezeRotation = false;
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Toilet"))
        {
            counter++;

            if(counter >= 10)
            {
                unClogged = true;
            }

            if (unClogged == true)
            {
                Debug.Log("wow");
                cloth.gameObject.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
