using UnityEngine;
using UnityEngine.InputSystem;

public class PauseModal : MonoBehaviour
{
    public static bool Handled = false;

    private bool _paused;
    
    public void Handle(InputAction.CallbackContext context)
    {
        if (!context.canceled)
        {
            return;
        }

        if (Handled)
        {
            Handled = false;
            return;
        }

        if (_paused)
        {
            _paused = false;
            gameObject.SetActive(false);
            TimeCheat.Instance.Resume();
            
            return;
        }

        Time.timeScale = 0;
        _paused = true;
        gameObject.SetActive(true);
    }
}