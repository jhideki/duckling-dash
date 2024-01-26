using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillDuckling : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    // Start is called before the first frame update
    public AudioSource audioSource;
    void Start()
    {
        anim = GetComponent<Animator>();
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
    }
}
