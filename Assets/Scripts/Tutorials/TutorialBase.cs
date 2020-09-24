using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBase : MonoBehaviour
{
    protected virtual void Start()
    {
        StartCoroutine(TutorialRoutine());
    }

    protected virtual IEnumerator TutorialRoutine() { yield return null; }

    public void FadeIn(GameObject obj, float seconds)
    {
        StartCoroutine(FadeInLerp(obj, seconds));
    }

    private IEnumerator FadeInLerp(GameObject obj, float seconds)
    {
        obj.SetActive(true);
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        float _t = 0;
        while (true)
        {
            _t += Time.deltaTime;
            renderer.color = new Color(1f, 1f, 1f, Mathf.Lerp(0, 1, _t / seconds));
        }
    }

    public void FadeOut(GameObject obj, float seconds)
    {
        StartCoroutine(FadeOutLerp(obj, seconds));
    }

    private IEnumerator FadeOutLerp(GameObject obj, float seconds)
    {
        obj.SetActive(true);
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        float _t = 0;
        while (true)
        {
            _t += Time.deltaTime;
            renderer.color = new Color(1f, 1f, 1f, Mathf.Lerp(1, 0, _t / seconds));
        }
    }
}
