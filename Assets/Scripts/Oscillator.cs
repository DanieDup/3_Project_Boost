using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    Vector3 movementVector = new Vector3(10f,0f,0f);
    //[Range(0, 1)] [SerializeField] float movementFactor; //0 for not moved, 1 for fully moved
    [SerializeField] float period = 2f;
    Vector3 startingPos;
    float movementFactor;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period >= Mathf.Epsilon) //smallest possible float
        {
            float cycles = Time.time / period;
            const float tua = Mathf.PI * 2;
            float rawSineWave = Mathf.Sin(cycles * tua); //goes from -1 to 1
            movementFactor = rawSineWave / 2f + 0.5f; //

            Vector3 offset = movementVector * movementFactor;
            transform.position = startingPos + offset;
        }

    }
}
