using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleProgress : MonoBehaviour
{
    public Image blueCircle1;
    // Start is called before the first frame update
    void Start()
    {
        blueCircle1.fillAmount = 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        blueCircle1.fillAmount = 0.6f;
    }
}
