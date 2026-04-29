using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    public GameObject escapePanel;
    private bool isOpen = false;
    public KeyCode EscapeButton;

    private void Update()
    {
        if (Input.GetKeyDown(EscapeButton))
            Toggle();
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        escapePanel.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
    }

    public void Resume()
    {
        isOpen = false;
        escapePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneTransition.instance.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}