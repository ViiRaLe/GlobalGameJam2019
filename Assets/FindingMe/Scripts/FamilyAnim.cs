using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamilyAnim : MonoBehaviour
{

    [SerializeField]
    private DragonBones.UnityArmatureComponent dbArmatureComponent;

    [SerializeField]
    private DragonBones.UnityArmatureComponent dbArmatureComponent2;

    public Player player;

    public GameObject casa;
    public GameObject final1;
    public GameObject final2;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(NewAnimationCoroutine(4.15f, "SECOND"));
    }



    private IEnumerator NewAnimationCoroutine(float delay, string anim)
    {
        yield return new WaitForSeconds(delay);
        dbArmatureComponent.animation.Play(anim);
        StartCoroutine(SwitchAnimationCoroutine(2.9f));
    }

    private IEnumerator SwitchAnimationCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        final1.SetActive(false);
        final2.SetActive(true);
        dbArmatureComponent2.animation.Play("FINALE");
        StartCoroutine(EndingAnimationCoroutine(2.9f));
    }

    private IEnumerator EndingAnimationCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        dbArmatureComponent2.animation.Stop();
        StartCoroutine(EsciLaCasa(0.5f));
    }

    private IEnumerator EsciLaCasa(float delay)
    {
        yield return new WaitForSeconds(delay);
        casa.GetComponent<Casa>().Parto();
    }

}