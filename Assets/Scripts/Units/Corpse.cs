using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    float lifetime = 30f;
    [SerializeField] string deathName = "death";
    [SerializeField]Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if (animator && !string.IsNullOrEmpty(deathName))
            animator.SetTrigger(deathName);
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
            Destroy(gameObject);
    }
}
