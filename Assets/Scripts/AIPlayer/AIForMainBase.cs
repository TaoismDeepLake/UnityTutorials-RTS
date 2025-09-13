using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIForMainBase : BaseAI
{
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
                    castingCooldown = 3f;
                }
            }
        }

    }
    
    public override bool isBusy()
    {
        return false;
    }
}
