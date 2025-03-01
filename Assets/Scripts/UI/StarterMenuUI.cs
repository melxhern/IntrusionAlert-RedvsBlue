
using UnityEngine;

public class StarterMenuUI : MonoBehaviour
{
    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
