using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuckCounter : MonoBehaviour
{
    private int numDucks;
    private int score;
    public Text numDucksUI;
    // Start is called before the first frame update
    void Start()
    {
        numDucks = 0;
        score = 0;

    }
    void Update()
    {
        //Debug.Log("num ducks" + numDucks);
        //Debug.Log("num score" + score);
    }

    public int GetNumDucks()
    {
        return numDucks;
    }
    public int GetScore()
    {
        return score;
    }


    public void IncrementDuckCount()
    {
        numDucks++;
    }
    public void DecrementDuckCount()
    {
        if (numDucks > 0)
        {
            numDucks--;
        }
    }
    public void SetScore()
    {
        score += numDucks;
        numDucks = 0;


        numDucksUI.text = score.ToString();
    }

    // Update is called once per frame
}
