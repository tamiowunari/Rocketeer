using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    //I initialized movementVector just to make it obvious whenever this script is added to an object.
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    float movementFactor;      //0 for not moved, 1 for fully moved
    Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //be able to set movementFactor automatically

        float cycles = Time.time / period;      //no. of cycles grows continually from 0 depending on the gameplay duration

        const float tau = 2 * Mathf.PI;     //2PI about 6.28
        float rawSineWave = Mathf.Sin(cycles * tau);       //oscillates from -1 to +1

        movementFactor = rawSineWave / 2 + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
