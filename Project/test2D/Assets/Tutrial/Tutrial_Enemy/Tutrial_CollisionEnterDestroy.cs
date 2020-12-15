using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutrial_CollisionEnterDestroy : MonoBehaviour
{
    [SerializeField] string TargetTagName = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == TargetTagName)
        {
            Destroy(this.gameObject);
        }
    }
}
