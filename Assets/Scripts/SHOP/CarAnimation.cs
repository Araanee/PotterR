using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 finalPosition;
    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
    } 

    // Update is called once per frame
    private void Update()
    {
        transform.position=Vector3.Lerp(transform.position,finalPosition,0.2f);
    }

    private void onDisable()
    {
        transform.position=initialPosition;
    }
}
