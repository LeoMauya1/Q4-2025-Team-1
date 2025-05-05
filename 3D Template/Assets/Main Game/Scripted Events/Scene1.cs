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
public class Scene1 : MonoBehaviour
{

    public Interactions[] interactions;
    public Animator transition;
    public Interactions player;
    public Image transitionScreen;
    public PlayableDirector director;
    public AudioSource audioSource;
    public AudioClip audioclip;
    public Monologue monologueSystem;
    private bool firstIteration;
    private bool secondIteration;
    private bool transitionOut;





    private void Start()
    {
        //transitionScreen.enabled = true;
        //StaticVariables.currentInteraction = player.gameObject;
        StartCoroutine(fadeInAni());
    }




    private IEnumerator fadeInAni()
    {
        
        yield return new WaitUntil(() => director.state != PlayState.Playing);
        audioSource.Play();
        yield return new WaitForSeconds(8f);
        player.MonologueInteraction();
    }

    private void Update()
    {
        if(monologueSystem.nextThought && !firstIteration && !secondIteration && !transitionOut)
        {
            StartCoroutine(nextThought());
        }
        if(monologueSystem.nextThought && firstIteration && !transitionOut)
        {
            StartCoroutine(nextThought2());
        }
        if( monologueSystem.nextThought && secondIteration)
        {
            StartCoroutine(fadeOut());
        }

        if (transition == null)
        {
            Debug.LogWarning("Animator 'transition' is not assigned!");
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

    private IEnumerator fadeOut()
    {
        if (transitionOut)
        {
            yield break;
        }
        transitionOut = true;
        secondIteration = false;
        yield return new WaitForSeconds(3);
        transition.SetBool("2", true);
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("Scene 2");

    }


}
