using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterMenuUI : MonoBehaviour
{
    public void OnClickOnlineButton()
    {
    }

    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}