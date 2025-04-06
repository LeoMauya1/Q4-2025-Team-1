using UnityEngine;

public class DukeLion : MonoBehaviour
{
    private Character dukeLion;

    public Dialogue dialogue;



    






    private void Update()
    {
       if( StaticVariables.hasQuestionableEvidence)
        {
            Answer();
        }
       
        
        
        
        
        
        
        
      









    }

    private void Answer()
    {
        foreach (PhysicalEvidence physical in StaticVariables.questionablEvidence)
        {
            if (physical.ItemName == "Letter")
            {
                dialogue.characterDialogue[0] = "ah, you found that letter.. well, I guess ill tell you the gist of it..";
            }
            else
            {
                Debug.Log("NO QUESTIONABLE EVIDENCE!");
            }




        }

    }



}

       