using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    public bool shadowIfItsInChildren = false;
    public Vector2 offset = new Vector2(0.05f, 0.05f);
    public Material shadowMaterial;
    public Color shadowColor;
    private SpriteRenderer sprRndCaster;
    private SpriteRenderer sprRndShadow;
    private Transform transCaster;
    private Transform transShadow;


    void Start()
    {
        transCaster = transform;
        transShadow = new GameObject().transform;

        if(!shadowIfItsInChildren){
            transShadow.localScale = transform.localScale;
            transShadow.parent = transCaster;
        }else{
            transShadow.parent = transCaster;   
            transShadow.localScale = transform.localScale;
        }
        
        transShadow.gameObject.name = "shadow";
        transShadow.localRotation = Quaternion.identity;

        if(GetComponent<SpriteRenderer>() != null) {
            sprRndCaster = GetComponent<SpriteRenderer>();
        }else{
            sprRndCaster = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        sprRndShadow = transShadow.gameObject.AddComponent<SpriteRenderer>();

        sprRndShadow.material = shadowMaterial;
        sprRndShadow.color = shadowColor;
        sprRndShadow.sortingLayerName = sprRndCaster.sortingLayerName;
        sprRndShadow.sortingOrder = -1; //sprRndCaster.sortingOrder - 1
    }

    void LateUpdate() {
        transShadow.position = new Vector2(transCaster.position.x + offset.x, transCaster.position.y + offset.y);
        sprRndShadow.sprite = sprRndCaster.sprite;
        sprRndShadow.flipX = sprRndCaster.flipX;
    }
}
