using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    //we initialize movementVector just to make it obvious when this script is added to an object.
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    float movementFactor;   //0 for not moved, 1 for fully moved
    Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //set movementFactor automatically

        if(period <= Mathf.Epsilon) { return; }         //protect against zero

        float cycles = Time.time / period;  //grows continually from 0 depending on the time duration you play

        const float tau = 2 * Mathf.PI; //2PI about 6.28
        float rawSineWave = Mathf.Sin(cycles * tau);    //oscillates from -1 to +1

        movementFactor = rawSineWave / 2 + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
