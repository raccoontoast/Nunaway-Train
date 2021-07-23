using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryNun : MonoBehaviour
{
    Animator animator;
    Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        AnimationCurve curve;
        Keyframe[] keys;

        keys = new Keyframe[3];
        keys[0] = new Keyframe(0f, 0f);
        keys[1] = new Keyframe(0.5f, 1f);
        keys[2] = new Keyframe(1f, 0f);

        curve = new AnimationCurve(keys);

        AnimationClip clip = new AnimationClip();
        clip.SetCurve("", typeof(Transform), "localPosition.y", curve);

        _transform = gameObject.transform;
        animator = GetComponent<Animator>();
        animator.speed = Random.Range(0.75f, 1.25f);
    }

    private void Update()
    {
        float y = gameObject.transform.position.y;
        gameObject.transform.position = new Vector3(_transform.position.x, y, _transform.position.z);
    }
}
