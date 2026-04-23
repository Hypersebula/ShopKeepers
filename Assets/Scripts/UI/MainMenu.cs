using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneTransition.instance.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}