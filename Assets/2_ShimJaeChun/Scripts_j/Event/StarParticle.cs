using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParticle : MonoBehaviour
{
    SpriteRenderer sprite;
    public float sizeSpeed = 1f;
    public float colorSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime * sizeSpeed);

        Color color = sprite.color;

        color.a = Mathf.Lerp(sprite.color.a, 0, Time.deltaTime * colorSpeed);

        sprite.color = color;

        if(sprite.color.a <= 0.01f)
        {
            Destroy(this.gameObject);
        }
    }
}
