using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splashController : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer spr;
    private Color32 clr;
    private float countAlpha = 255;

    // Start is called before the first frame update
    void Start()
    {
        float rnd = Random.Range(0, sprites.Length-1);
        spr.sprite = sprites[(int)rnd];
        clr = spr.color;
    }

    // Update is called once per frame
    void Update()
    {
        countAlpha -= Time.deltaTime*20;
        if(countAlpha > 0){
            spr.color = new Color32(clr.r, clr.g, clr.b, (byte)countAlpha);
        }else{
            Destroy(this.gameObject);
        }
    }
}
