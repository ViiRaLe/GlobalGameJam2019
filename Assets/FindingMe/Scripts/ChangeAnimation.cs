using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimation : MonoBehaviour
{
    public GameObject dialog;
    public AnimationClip endClip;

    private void Update()
    {
        if (dialog.GetComponent<Animation>().enabled == false)
        {
            GetComponent<Animation>().clip = endClip;

            GetComponent<Animation>().Play();
        }
    }
}
