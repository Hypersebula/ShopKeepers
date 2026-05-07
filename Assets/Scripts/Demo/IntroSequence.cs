using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroSequence : MonoBehaviour
{
    public TextMeshProUGUI[] textPages;
    public float letterDelay = 0.05f;
    private int currentPage = 0;
    private bool isAnimating = false;
    public int nextSceneIndex = 1;
    public GameObject skipPrompt;

    private void Start()
    {
        StartCoroutine(ShowPage(0));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isAnimating)
            {
                StopAllCoroutines();
                textPages[currentPage].maxVisibleCharacters = textPages[currentPage].text.Length;
                isAnimating = false;
                skipPrompt.SetActive(true);
            }
            else
                StartCoroutine(NextPage());
        }
    }

    private IEnumerator NextPage()
    {
        skipPrompt.SetActive(false);
        currentPage++;
        if (currentPage >= textPages.Length)
        {
            if (SceneTransition.instance != null)
                SceneTransition.instance.LoadScene(nextSceneIndex);
            else
                SceneManager.LoadScene(nextSceneIndex);
            yield break;
        }
        yield return ShowPage(currentPage);
    }

    private IEnumerator ShowPage(int index)
    {
        for (int i = 0; i < textPages.Length; i++)
            textPages[i].gameObject.SetActive(i == index);

        TextMeshProUGUI text = textPages[index];
        int totalChars = text.text.Length;
        text.maxVisibleCharacters = 0;
        isAnimating = true;

        for (int i = 0; i <= totalChars; i++)
        {
            text.maxVisibleCharacters = i;
            yield return new WaitForSeconds(letterDelay);
        }
        isAnimating = false;
        skipPrompt.SetActive(true);
    }
}