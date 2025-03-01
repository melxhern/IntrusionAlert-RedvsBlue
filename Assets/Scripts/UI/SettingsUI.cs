using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField]
    private Button MouseControllButton;
    [SerializeField]
    private Button KeyboardMouseControllButton;
    [SerializeField]
    private Button JoyStickButton;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // private void OnEnable()
    // {
    //     switch (PlayerSettings.controlType)
    //     {
    //         case EControlType.Mouse:
    //             MouseControllButton.image.color = Color.green;
    //             KeyboardMouseControllButton.image.color = Color.white;
    //             JoyStickButton.image.color = Color.white;
    //             break;

    //         case EControlType.KeyboardMouse:
    //             KeyboardMouseControllButton.image.color = Color.green;
    //             MouseControllButton.image.color = Color.white;
    //             JoyStickButton.image.color = Color.white;
    //             break;

    //         case EControlType.JoyStick:
    //             JoyStickButton.image.color = Color.green;
    //             MouseControllButton.image.color = Color.white;
    //             KeyboardMouseControllButton.image.color = Color.white;
    //             break;
    //     }
    // }

    // public void SetControllMode(int controlType)
    // {
    //     PlayerSettings.controlType = (EControlType)controlType;
    //     switch (PlayerSettings.controlType)
    //     {
    //         case EControlType.Mouse:
    //             MouseControllButton.image.color = Color.green;
    //             KeyboardMouseControllButton.image.color = Color.white;
    //             JoyStickButton.image.color = Color.white;
    //             break;

    //         case EControlType.KeyboardMouse:
    //             KeyboardMouseControllButton.image.color = Color.green;
    //             MouseControllButton.image.color = Color.white;
    //             JoyStickButton.image.color = Color.white;
    //             break;

    //         case EControlType.JoyStick:
    //             JoyStickButton.image.color = Color.green;
    //             MouseControllButton.image.color = Color.white;
    //             KeyboardMouseControllButton.image.color = Color.white;
    //             break;
    //     }
    // }

    // Close the settings UI with a delay
    public virtual void Close()
    {   
        // Start the coroutine to close the UI after a delay
        StartCoroutine(CloseAfterDelay());
    }

    // Coroutine to handle the delayed closing of the settings UI
    private IEnumerator CloseAfterDelay()
    {
        // Trigger the close animation
        animator.SetTrigger("close");

        // Wait for the animation to complete
        yield return new WaitForSeconds(0.5f);

        // Deactivate the game object
        gameObject.SetActive(false);

        // Reset the close trigger
        animator.ResetTrigger("close");
    }
}
