using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        float jump = Input.GetAxis("Vertical");
        transform.Translate(move * 2.0f * Time.deltaTime, jump * 2.0f * Time.deltaTime, 0.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("collided with " + collision.gameObject.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collided with " + other.gameObject.name);
    }
}
