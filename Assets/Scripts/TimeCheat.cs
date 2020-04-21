using TMPro;
using UnityEngine;

public class TimeCheat : MonoBehaviour
{
    public static TimeCheat Instance;

    public TextMeshProUGUI buttonText;

    private bool _fastMode;

    private void Awake()
    {
        Instance = this;
    }

    public void Toggle()
    {
        _fastMode = !_fastMode;
        Time.timeScale = _fastMode ? 2 : 1;
        buttonText.text = _fastMode ? ">>" : ">";
    }

    public void Resume()
    {
        Time.timeScale = _fastMode ? 2 : 1;
    }

    public bool IsFastMode() => _fastMode;
}