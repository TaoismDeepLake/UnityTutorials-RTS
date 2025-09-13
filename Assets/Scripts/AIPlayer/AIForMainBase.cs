using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIForMainBase : BaseAI
{
    public float buildingCooldown = 10f;
    public override void RunAITick()
    {
        if (castingCooldown <= 0)
        {
            int skillCount = skillManagers.Count;
            if (skillCount > 0)
            {
                SkillManager sm = skillManagers[Random.Range(0, skillCount)];
                if (sm != null)
                {
                    sm.Trigger();
                    castingCooldown = buildingCooldown;
                }
            }
        }

    }
    
    public override bool isBusy()
    {
        return false;
    }
}
