using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Arrow")
            return;

        var width = GetComponent<SpriteRenderer>().bounds.size.x;
        var distance = Mathf.Abs(col.transform.position.x - transform.position.x);

        if (distance >= width/2)
            col.gameObject.GetComponent<Arrow>().Deflect(-1, 1);
        else if (col.transform.position.y > transform.position.y)
            col.gameObject.GetComponent<Arrow>().Deflect(1, -0.75f);
        else
            col.gameObject.GetComponent<Arrow>().Deflect(1, -0.9f);
    }
}
