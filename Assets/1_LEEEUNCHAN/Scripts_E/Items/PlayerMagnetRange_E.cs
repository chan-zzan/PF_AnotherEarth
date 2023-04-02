using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagnetRange_E : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            collision.GetComponent<MagnetEffect_E>().MagnetEffect();
        }
    }
}
