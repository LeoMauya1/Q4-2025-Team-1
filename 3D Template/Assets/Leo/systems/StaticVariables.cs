using UnityEngine;
using System.Collections.Generic;

public class StaticVariables
{
    public static int storyArc;
    public static GameObject currentInteraction;
    public static GameObject runnerUpInteraction;
    public static bool selectedSexisMale;
    public static bool selectedSexisFemale;
    public static bool canInteract;
    public static bool hasQuestionableEvidence;
    public static List<PhysicalEvidence> questionablEvidence = new List<PhysicalEvidence>();
    public static bool isConversing = false;
    public static bool initialInteraction;
    public static bool nextInteraction;
    public static bool sentenceCompletion;
    public static bool gamePaused;
    public static bool itemInteraction;
    public static bool promptInterogation;
    public static GameObject dragAndDropInteraction;
   
}
