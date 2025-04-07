using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TitleFadeInOut : MonoBehaviour
{
    bool isTouch = false;
    private TextMeshProUGUI tmpText;
    private Color originalColor;

    public float fadeDuration = 1.0f;

    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject Panel;

    void Start()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        originalColor = tmpText.color;

        StartCoroutine(FadeLoop());
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTouch)
        {
            isTouch = true;
            SoundManager.Instance.PlayUISfxShot(0); // Edit
            StopAllCoroutines();
            Title.SetActive(false);
            Panel.SetActive(true);
        }
    }

    IEnumerator FadeLoop()
    {
        while (!isTouch)
        {
            yield return StartCoroutine(Fade(1f, 0f));
            yield return StartCoroutine(Fade(0f, 1f));
        }
    }

    IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            Color c = originalColor;
            c.a = alpha;
            tmpText.color = c;

            elapsed += Time.deltaTime;
            yield return null;
        }

        Color finalColor = originalColor;
        finalColor.a = to;
        tmpText.color = finalColor;
    }
}
