using UnityEngine;
using UnityEngine.InputSystem;

public class dialogueProgressionNoplayer : MonoBehaviour
{

    private ActionMaps actionMap;
    public InputAction click;
    public InputAction skip;


    private void Awake()
    {
        actionMap = new ActionMaps();
    }



    private void OnEnable()
    {
        click = actionMap.PlayerActions.Click;
        skip = actionMap.PlayerActions.Skip;
        click.Enable();
        skip.Enable();

        click.performed += Interact;
        skip.performed += SkipText;
    }

    private void OnDisable()
    {
        click.Disable();
        skip.Disable();
        click.performed -= Interact;
        skip.performed -= SkipText;
    }



















    private void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("click!");

        if (StaticVariables.isConversing && StaticVariables.initialInteraction && StaticVariables.sentenceCompletion == true)
        {
            FindAnyObjectByType<Monologue>().nextSentence();
        }

     




    }

    private void SkipText(InputAction.CallbackContext context)
    {
        if (StaticVariables.isConversing && StaticVariables.initialInteraction && StaticVariables.dialogueTextprogressing)
        {
            FindAnyObjectByType<Monologue>().sentenceSkipped = true;
        }
    }

    private T GetcCurrentInteractionComponent<T>() where T : MonoBehaviour
    {
        var c_components = StaticVariables.currentInteraction.GetComponents<T>();

        foreach (var interaction in c_components)
        {
            if (interaction.enabled)
            {
                return interaction;
            }
        }
        return null;
    }

}
