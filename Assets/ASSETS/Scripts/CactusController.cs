using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusController : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer spr;
    public GameObject effect;

    void Start() {
        spr.sprite = sprites[Random.Range(2, sprites.Length*2)/2];
    }

    public void cut(){
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteShadow>().offset = Vector2.one * 0.01f;
        Instantiate(effect, transform.position, effect.transform.rotation);
        spr.sprite = sprites[0];
        spr.sortingOrder = 0;
    }
}