using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casa : MonoBehaviour
{
    private bool vado = false;
    public Camera cam;

    public GameObject fade;
    public GameObject fader;
    public GameObject end;
    public GameObject canvas;
    public GameObject soundManager;


    private bool casaDelayer = false;
    private bool start = true;
    private bool camGo = false;
    private float t = 0f;

    public void Parto()
    {
        vado = true;
    }

    private void Update()
    {
        if (vado && transform.position.y < -54f)
        {
            if (camGo)
            {
                transform.Translate(0, 0.15f, 0);
            }
            else
            {
                camGo = true;
                cam.orthographicSize = 40f;
                cam.transform.position = new Vector3(cam.transform.position.x - 18f, cam.transform.position.y + 25f, cam.transform.position.z);
                fade.SetActive(false);
                StartCoroutine(CasaDelayer(5f));
            }
        }
        else if (t <= 400f && camGo && casaDelayer)
        {
            t += Time.deltaTime;
            fader.GetComponent<SpriteRenderer>().color += new Color(255f, 255f, 255f, t / 400f);
            end.GetComponent<SpriteRenderer>().color += new Color(255f, 255f, 255f, t / 500f);
            if (start)
            {
                start = false;
                Coroutine xd = StartCoroutine(soundManager.GetComponent<AudioFadeOut>().FadeOut(cam.GetComponent<AudioSource>(), 40f));
            }
        }

        if (end.GetComponent<SpriteRenderer>().color.a >= 1f)
        {
            canvas.SetActive(true);
        }
    }

    IEnumerator CasaDelayer(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        casaDelayer = true;
    }
}
