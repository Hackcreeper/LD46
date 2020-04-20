using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;
    
    public GameObject[] dialogs;
    public GameObject sleepingQuartersArrow;
    public Button sleepingQuartersButton;
    public GameObject gardenArrow;
    public Button gardenButton;
    public GameObject buildingModalArrow;

    public Button[] buildingButtons;
    
    private int _step = 1;
    private bool _active = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartStep();
    }

    private void StartStep()
    {
        // Reset
        sleepingQuartersArrow.SetActive(false);
        gardenArrow.SetActive(false);
        buildingModalArrow.SetActive(false);
        
        foreach (var dialog in dialogs)
        {
            dialog.SetActive(false);
        }
        
        // Enable next dialog
        dialogs[_step-1].SetActive(true);

        // Step specific
        switch (_step)
        {
            case 2: // Sleeping quarters
                sleepingQuartersArrow.SetActive(true);
                break;
            
            case 3: // Garden
                gardenArrow.SetActive(true);
                gardenButton.interactable = true;
                break;
            
            case 4: // Assign to garden
                buildingModalArrow.SetActive(true);
                break;
            
            case 5: // Done broi
                break;
        }
    }

    public bool IsActive() => _active;

    public void FinishStep1()
    {
        _step++;
        StartStep();
    }

    public void FinishSleepingQuarterStep()
    {
        sleepingQuartersButton.interactable = false; 
        
        _step++;
        StartStep();
    }

    public void FinishGardenStep()
    {
        gardenButton.interactable = false;

        _step++;
        StartStep();
    }

    public void FinishAssignGardenStep()
    {
        _step++;
        
        StartStep();
    }

    public bool CanOpenGarden() => _step == 4;

    public void EndTutorial()
    {
        _active = false;

        foreach (var buildingButton in buildingButtons)
        {
            buildingButton.interactable = true;
        }

        foreach (var dialog in dialogs)
        {
            dialog.SetActive(false);
        }
    }
}