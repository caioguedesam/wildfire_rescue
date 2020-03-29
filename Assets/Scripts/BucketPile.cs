using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketPile : MonoBehaviour
{
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void BucketPoured() {
        animator.SetTrigger("bucketPoured");
    }
}
