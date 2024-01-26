using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    Animator anim;
    PlayerDown playerDown;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerDown = GetComponent<PlayerDown>();

    }

    public void Die()
    {
        StartCoroutine(PlayAnimation());
    }
    IEnumerator PlayAnimation()
    {
        audioSource.Play();
        anim.enabled = true;
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        playerDown.LoadGameOverScene();
    }

    // Update is called once per frame
}
