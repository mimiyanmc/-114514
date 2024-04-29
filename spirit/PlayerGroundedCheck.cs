using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedCheck : MonoBehaviour
{
    PlayerController controller;
    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == controller.gameObject)
        {
            return;
        }
        controller.SetGroundedState(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject==controller.gameObject) {
            return;
        }
        controller.SetGroundedState(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == controller.gameObject)
        {
            return;
        }
        controller.SetGroundedState(true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == controller.gameObject)
        {
            return;
        }
        controller.SetGroundedState(true);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == controller.gameObject)
        {
            return;
        }
        controller.SetGroundedState(false);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == controller.gameObject)
        {
            return;
        }
        controller.SetGroundedState(true);
    }
}
