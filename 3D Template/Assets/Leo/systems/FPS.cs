using UnityEngine;
using UnityEngine.InputSystem;

public class FPS : MonoBehaviour
{
    private ActionMaps inputActions;
    public InputAction look;
    float xRotation = 0f;
    public Transform player;
    public float mouseSensitivity;




    private void Awake()
    {
        inputActions = new ActionMaps();
    }
    private void OnEnable()
    {
        look = inputActions.PlayerActions.Look;
        look.Enable();
    }



    private void OnDisable()
    {
        look.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        float mouseX = look.ReadValue<Vector2>().x * mouseSensitivity * Time.deltaTime;
        float mouseY = look.ReadValue<Vector2>().y * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);

    }
}
