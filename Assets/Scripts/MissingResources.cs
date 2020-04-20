using System;
using Resource;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissingResources : MonoBehaviour
{
    public static MissingResources Instance;

    public GameObject suffocatingWarning;
    public GameObject starvingWarning;
    public GameObject diedWarning;
    
    public TextMeshProUGUI diedText;
    
    private int _reportedO2 = 0;
    private int _reportedFood = 0;

    private bool _someoneDied;
    private float _deadTimer;
    
    private void Awake()
    {
        Instance = this;
    }

    public void ReportO2()
    {
        _reportedO2++;
        
        suffocatingWarning.SetActive(true);
    }

    public void ReportFood()
    {
        _reportedFood++;
        
        starvingWarning.SetActive(true);
    }

    public void ClearReportO2(int amount)
    {
        _reportedO2 -= amount;

        if (_reportedO2 == 0)
        {
            suffocatingWarning.SetActive(false);
        }
    }
    
    public void ClearReportFood(int amount)
    {
        _reportedFood -= amount;
        
        starvingWarning.SetActive(false);
    }

    public void Died(string reason, int o2, int food)
    {
        diedText.text = $"Warning: A colonist has died because of {reason}!";
        diedWarning.SetActive(true);
        
        _reportedFood -= food;
        _reportedO2 -= o2;

        _someoneDied = true;
        _deadTimer = 10f;
    }

    private void Update()
    {
        if (!_someoneDied)
        {
            return;
        }

        _deadTimer -= Time.deltaTime;
        if (_deadTimer > 0)
        {
            return;
        }

        _someoneDied = false;
        diedWarning.SetActive(false);
    }
}