using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpParticleEvent : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(ParticleTimer());
    }

    IEnumerator ParticleTimer()
    {
        yield return new WaitForSeconds(0.8f);

        Destroy(this.gameObject);
    }
}
