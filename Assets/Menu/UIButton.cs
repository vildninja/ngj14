using UnityEngine;
using System.Collections;

public class UIButton : MonoBehaviour {

    protected bool down;
    bool old;

    public AudioClip clip;

    public exSprite animate;
    public AnimationCurve fade;
    float downTime;

	// Use this for initialization
	void Awake () {
        down = false;
        old = false;
	}

    public virtual void Press(TouchPhase phase)
    {
        if (old && phase == TouchPhase.Ended)
        {
            down = false;
            old = false;
            Up();
        }
        else if (phase == TouchPhase.Began)
        {
            down = true;
            Down();
        }
        else if (old)
        {
            down = true;
        }
    }

    protected virtual void Down()
    {

    }

    protected virtual void Up()
    {

    }

    protected virtual void Canceled()
    {

    }
	
	// Update is called once per frame
	protected void Update () {
        if (old != down)
        {
            old = down;
            if (!down)
                Canceled();
            else if (clip)
                AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        }
        if (animate)
        {
            downTime = Mathf.Clamp(downTime + (down ? Time.deltaTime : -Time.deltaTime / 6), 0, fade[fade.length - 1].time);
            var c = animate.color;
            c.a = fade.Evaluate(downTime);
            animate.color = c;
        }
        down = false;
	}
}
