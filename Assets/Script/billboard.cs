using System.Collections;
using UnityEngine;
using TMPro;

public class CatGuide : MonoBehaviour
{
    public GameObject speechBubble;
    public TextMeshProUGUI speechText;

    private Coroutine currentCoroutine;

    void Start()
    {
        if (speechBubble != null) speechBubble.SetActive(false);
    }

    public void DonnerConseil(string message, float duree = 4f)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ShowMessageRoutine(message, duree));
    }

    private IEnumerator ShowMessageRoutine(string message, float duree)
    {
        if (speechBubble != null && speechText != null)
        {
            speechText.text = message;
            speechBubble.SetActive(true);

            yield return new WaitForSeconds(duree);

            speechBubble.SetActive(false);
        }
    }
}