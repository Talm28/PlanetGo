using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioClip outSound;

    public void EndScene(string sceneName)
    {
        Time.timeScale = 1f; // If game was puased, continue time
        StartCoroutine(Transition(sceneName));
    }

    IEnumerator Transition(string sceneName)
    {
        animator.SetTrigger("Activate");
        
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = outSound;
        audioSource.Play();

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
