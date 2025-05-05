using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class Scene2 : MonoBehaviour
{


    public PlayableDirector director;

    public Monologue monologueSystem;
    public Interactions player;
    private bool firstIteration;
    private bool secondIteration;
    private bool transitionOut;



    private void Update()
    {
        if (monologueSystem.nextThought && !firstIteration && !secondIteration && !transitionOut)
        {
            StartCoroutine(nextThought());
        }
        if (monologueSystem.nextThought && firstIteration && !transitionOut)
        {
            StartCoroutine(nextThought2());
        }
       
    }



    private IEnumerator nextThought()
    {
        monologueSystem.nextThought = false;
        firstIteration = true;
        yield return new WaitForSeconds(3f);
        player.nextStreamOfConscious();


    }
    private IEnumerator nextThought2()
    {
        monologueSystem.nextThought = false;
        firstIteration = false;
        monologueSystem.secondIteration = true;
        yield return new WaitForSeconds(3f);
        player.nextStreamOfConscious2();
        secondIteration = true;

    }


}
