using UnityEngine;
using System.Collections.Generic;

public class CDMANAGER : MonoBehaviour
{
  public List<Interactions> characterDialogueList;
    public List<FollowUpInteraction> characterFollowUpDialogueList;
    private int eventValue;


    private void Update()
    {
     

      for(int i = 0; i< characterDialogueList.Count; i++)
        {
            if(i == StaticVariables.eventValue)
            {
                characterDialogueList[i].enabled = true;

            }
            else
            {
                characterDialogueList[i].enabled = false;
              
            }
        }
    }

    private void UpdateDialogue()
    {

    }

}
