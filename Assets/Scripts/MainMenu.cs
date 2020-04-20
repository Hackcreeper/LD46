using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool _starting = false;
    
    private void Update()
    {
        if (!Input.anyKeyDown || _starting)
        {
            return;
        }

        _starting = true;
        GetComponent<Animator>().SetBool("flying", true);
    }

    public void SwitchLevel()
    {
        SceneManager.LoadScene("Game");
    }
}