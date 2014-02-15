using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {

    public static Vector3 inputPosition
    {
        get;
        private set;
    }
	
	// Update is called once per frame
    public void Update()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
        foreach (Touch t in Input.touches) {
            TouchEvent(t.position, t.phase, t.fingerId);
		}
#else
        if (Input.GetMouseButtonDown(0))
            TouchEvent(Input.mousePosition, TouchPhase.Began, 0);
        else if (Input.GetMouseButtonUp(0))
            TouchEvent(Input.mousePosition, TouchPhase.Ended, 0);
        else if (Input.GetMouseButton(0))
            TouchEvent(Input.mousePosition, TouchPhase.Moved, 0);
#endif
    }


    void TouchEvent(Vector2 pos, TouchPhase phase, int id) {
        TouchEvent(new Vector3(pos.x, pos.y), phase, id);
    }

    void TouchEvent(Vector3 pos, TouchPhase phase, int id)
    {
        inputPosition = pos;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.GetComponent<UIButton>())
                hit.transform.GetComponent<UIButton>().Press(phase);
            CustomTouchEvent(pos, phase, id, hit);
        }
        else
        {
            CustomTouchEvent(pos, phase, id, null);
        }
    }

    protected virtual void CustomTouchEvent(Vector3 pos, TouchPhase phase, int id, RaycastHit? hit)
    {

    }
}
