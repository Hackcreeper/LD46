using Resource;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    private void Update()
    {
        if (ResourceManager.Instance.ForType(ResourceType.Colonists).Get() >= 50)
        {
            SceneManager.LoadScene("Win");
        }
    }
}