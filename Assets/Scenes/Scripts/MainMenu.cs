using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        // Если в редакторе — просто сообщение
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Если в собранной игре — выход
        Application.Quit();
#endif
    }
}