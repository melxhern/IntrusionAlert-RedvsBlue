using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Update()
    {
        // Déverrouiller le curseur dès le lancement du jeu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
