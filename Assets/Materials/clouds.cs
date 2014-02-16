using UnityEngine;
using System.Collections;

public class clouds : MonoBehaviour {
    public float alpha = 0f;
    bool increaseAlpha = true;
    float posX, posY, posZ;
    float scaleX, scaleY, scaleZ;
    public float scaleSpeed;
    public float moveSpeed;
	// Use this for initialization
	void Start () {

        posX = transform.position.x;
        posY = transform.position.y;
        posZ = transform.position.z;

        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
        scaleZ = transform.localScale.z;

	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
        transform.position += new Vector3(moveSpeed, moveSpeed, 0);

        if (increaseAlpha == true)
        {
            alpha += 0.001f;
            

        }

        else alpha -= 0.001f;

        renderer.material.color = new Color(1, 1, 1, alpha);

        if (alpha > 0.3f)
        {
            increaseAlpha = false;
            
        }

        if  (alpha <= 0f)
        {
            increaseAlpha = true;
            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            transform.position = new Vector3(posX,posY,posZ);
            
        }
         

	}
}
