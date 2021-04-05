using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScore : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI cashText;

    // Start is called before the first frame update
    void Start()
    {
        waveText.text = "Made it to Wave " + ScoreManager.waveNumber;
        cashText.text = "Cash: $" + ScoreManager.cashNumber;
    }
}
