using UnityEngine;
using UnityEngine.Timeline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Scene3 : MonoBehaviour
{
    public PlayableDirector director;

    public Monologue monologueSystem;
    public Interactions player;
    public dialogueProgressionNoplayer playerController;
    private bool firstIteration;
    private bool secondIteration;
    private bool transitionOut;
    private bool pause;
    private bool monologueComplete;
    private double frameRate;
    private bool firstTransition;
    private bool secondTransition;



    public Interactions lion;
    public Interactions meow;
    public Interactions maid;
    public Interactions wolf;
    public Interactions bear;


    private void Awake()
    {
        TimelineAsset timeline = director.playableAsset as TimelineAsset;

        if (timeline != null)
        {
            frameRate = timeline.editorSettings.frameRate;
            Debug.Log($"current framerate is at { frameRate}");
        }
    }

    private void Update()
    {
      TimelineAsset timeline = director.playableAsset as TimelineAsset;

        //nextSquence();

     

        if (director.state == PlayState.Paused && monologueComplete)
        {
            director.Play();
        }



        if (monologueSystem.nextThought && monologueSystem.currentMonologue.queueCount == 1)
        {
            setTime(822);
            director.Evaluate();
            director.Play();
            monologueSystem.nextThought = false;
            firstTransition = true;
            playerController.clicked = false;



        }


        if(monologueSystem.nextThought && monologueSystem.currentMonologue.queueCount == 2)
        {
            setTime(1970);
            director.Evaluate();
            director.Play();
            monologueSystem.nextThought = false;
            firstTransition = true;
            playerController.clicked = false;

        }






        if (monologueSystem.nextThought && monologueSystem.currentMonologue.queueCount == 3 && playerController.clicked && director.time >= 58)
        {
            Debug.Log("yes!");
            director.Stop();
            StaticVariables.currentInteraction = lion.gameObject;
            lion.dialogueInteraction();


        }
    
    }

    private bool nextSquence()
    {
        double targetTime = 1135.0;
        double epsilon = 0.1;

        return System.Math.Abs(director.time - targetTime) < epsilon;
    }



    private IEnumerator nextThought()
    {
        monologueSystem.nextThought = false;
        firstIteration = true;
        yield return new WaitUntil(() => nextSquence());
  


    }
   

    public void FreezDirector()
    {
        director.Stop();
    }




    private void setTime(int frame)
    {
        double Time = frame;
        director.time = Time / frameRate;
    }

    private double GrabTime(double time)
    {
        return time / frameRate;
    }
}
