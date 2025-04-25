using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerTabUI : MonoBehaviour
{
    public InputAction tab;
    private ActionMaps actionMaps;
    public GameObject playerTab;
    public GameObject reticle;
    public GameObject gameobejcttt;
    public GameObject itemPage;
    public Image itemPageImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDisc;


    private void Awake()
    {
        actionMaps = new ActionMaps();
    }


    private void OnEnable()
    {
        tab = actionMaps.UI.PlayerTab;
        tab.Enable();
        tab.performed += PlayerTabOpened;

    }
    private void OnDisable()
    {
        tab.Disable();
        tab.performed -= PlayerTabOpened;
    }

    private void PlayerTabOpened(InputAction.CallbackContext context)
    {
        StaticVariables.currentInteraction = null;
        StaticVariables.runnerUpInteraction = null;
        bool isActive = !playerTab.activeSelf;
     
        playerTab.SetActive(!playerTab.activeSelf);
        reticle.SetActive(!playerTab.activeSelf);
       
      
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        StaticVariables.gamePaused = isActive ? StaticVariables.gamePaused = true : StaticVariables.gamePaused = false;


    }

    public void tabOutInteraction()
    {
        bool isActive = !playerTab.activeSelf;
        playerTab.SetActive(!playerTab.activeSelf);
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        StaticVariables.gamePaused = isActive ? StaticVariables.gamePaused = true : StaticVariables.gamePaused = false;
    }























}
