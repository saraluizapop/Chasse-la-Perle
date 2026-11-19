using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void RetournerAuMenu(string nomSceneMenu = "MainMenu")
    {
        // S'assure que le temps n'est pas en pause et que la souris est visible
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(nomSceneMenu);
    }
}