using UnityEngine;
using UnityEngine.InputSystem;

public class FPS : MonoBehaviour
{
    private ActionMaps inputActions;
    public InputAction look;
    public InputAction W_nav;
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
        W_nav = inputActions.UI.MenuGamespacenavigation;
        look.Enable();
        W_nav.Enable();
    }



    private void OnDisable()
    {
        look.Disable();
        W_nav.Disable();
    }


    // Update is called once per frame
    void Update()
    {

        if(StaticVariables.isConversing == false && StaticVariables.gamePaused == false)
        {
            float mouseX = look.ReadValue<Vector2>().x * mouseSensitivity * Time.deltaTime;
            float mouseY = look.ReadValue<Vector2>().y * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);
        }
        if (StaticVariables.gamePaused == true)
        {
            Vector2 naivgation = W_nav.ReadValue<Vector2>();
            float rotateSpeed = 50f;
            transform.Rotate(Vector3.up, naivgation.x * rotateSpeed * Time.unscaledDeltaTime, Space.World);
            transform.Rotate(Vector3.right,-naivgation.y * rotateSpeed * Time.unscaledDeltaTime, Space.Self);
           
        }
        if(StaticVariables.isConversing == true)
        {

        }

    }
}
