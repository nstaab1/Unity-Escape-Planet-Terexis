using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 0f,0f);
    [SerializeField] float period = 2f;

    float movementFactor; // 0 not move 1 for fully moved

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        movementFactor = CalculateMovementFactor();
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }

    private float CalculateMovementFactor()
    {
        float cycles = Time.time / period; // grows continually from 0
        const float tau = Mathf.PI * 2; // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);
        return rawSinWave / 2f + 0.5f;
    }
}
