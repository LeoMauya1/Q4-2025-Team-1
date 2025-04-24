using System.Collections;
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
    public LayerMask puzzleLayer;
    public GameObject particle;
    public int difCounter = 0;

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
        var raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
         if (!Physics.Raycast(raycast, 999, puzzleLayer))
        {
            GetComponent<Rigidbody>().position = NewWorldPosition;
        }

        
    }

     IEnumerator WaterParticles()
    {
        particle.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.5f);
        particle.GetComponent<ParticleSystem>().Stop();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Toilet"))
        {
            counter++;
            Debug.Log(counter);
            


            if(counter >= 10)
            {
                unClogged = true;
                
            }

            if (unClogged == true)
            {
                Debug.Log("wow");
                cloth.gameObject.SetActive(true);
            }

            if(difCounter < counter && counter <= 10)
            {
                StartCoroutine(WaterParticles());
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
