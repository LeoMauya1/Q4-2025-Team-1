using UnityEngine;
using System.Collections.Generic;

public class StaticVariables
{
    public static int storyArc;
    public static GameObject currentInteraction;
    public static bool selectedSexisMale;
    public static bool selectedSexisFemale;
    public static bool canInteract;
    public static bool hasQuestionableEvidence;
    public static List<PhysicalEvidence> questionablEvidence = new List<PhysicalEvidence>();
    public static bool isConversing = false;
   
}
