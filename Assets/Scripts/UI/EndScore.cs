using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScore : MonoBehaviour
{
    public Text score;
    public Score playerScore;
    // Start is called before the first frame update
    void Start()
    {
        score.text = "Score: " + playerScore.intValue.ToString();
    }
}
