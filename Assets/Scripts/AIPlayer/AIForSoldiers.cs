using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIForSoldiers : BaseAI
{
    float wanderCooldown = 0f;
    public bool wanderEnabled = true;

    CharacterManager characterManager;
    public void Start()
    {
        base.Start();
        //if unit manager is not character manager, disable script
        if (!(unitManager is CharacterManager))
        {
            Debug.LogError("No CharacterManager found on " + gameObject.name);
            enabled = false;
            return;
        }
        characterManager = unitManager as CharacterManager;
    }

    public override void RunAITick()
    {
        if (characterManager == null) return;
        if (isBusy()) return;
        //wander around randomly
        if (wanderEnabled)
        {
            if (wanderCooldown <= 0)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 5f;
                randomDirection += characterManager.transform.position;
                UnityEngine.AI.NavMeshHit hit;
                UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, 5f, 1);
                characterManager.MoveTo(hit.position);
                wanderCooldown = Random.Range(3f, 10f);
            }
            else
            {
                wanderCooldown -= Time.deltaTime;
                if (wanderCooldown < 0) wanderCooldown = 0;
            }
        }
        
    }

    public override bool isBusy()
    {
        if (characterManager == null) return true;
        return characterManager.agent.hasPath;
    }
}
