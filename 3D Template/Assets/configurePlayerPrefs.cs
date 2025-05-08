using UnityEngine;

public class configurePlayerPrefs : MonoBehaviour
{


    public void chooseGirl()
    {
        StaticVariables.selectedSexisFemale = true;
    }

    public void chooseBoy()
    {
        StaticVariables.selectedSexisMale = true;
    }

    public void SetName( string name)
    {
        StaticVariables.playerName = name;
    }

    private void Update()
    {
       if(StaticVariables.selectedSexisMale && StaticVariables.playerName != null)
        {
            Debug.Log($" you have selected he/him/his pronouns! and your name is {StaticVariables.playerName}");
        }
        if (StaticVariables.selectedSexisFemale && StaticVariables.playerName != null)
        {
            Debug.Log($" you have selected she/her/hers pronouns! and your name is {StaticVariables.playerName}");
        }


    }
}
