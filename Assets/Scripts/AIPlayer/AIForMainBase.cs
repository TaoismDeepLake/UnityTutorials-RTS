using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIForMainBase : BaseAI
{
    public float buildingCooldown = 10f;
    public float hordeAttackCooldown = 60f;
    [SerializeField] float curHordeCooldown = 0f;
    public override void RunAITick()
    {
        if (castingCooldown <= 0)
        {
            int skillCount = skillManagers.Count;
            if (skillCount > 0)
            {
                int index = Random.Range(0, skillCount);
                Debug.Log("AIForMainBase: Total skills: " + skillCount + ", selected index: " + index);
                SkillManager sm = skillManagers[index];
                if (sm != null)
                {
                    sm.Trigger();
                    castingCooldown = buildingCooldown;
                }
            }
        }

        if (curHordeCooldown <= 0f)
        {
            if (HordeAttack())
            {
                curHordeCooldown = hordeAttackCooldown;
            }
        }
        else
        {
            curHordeCooldown -= Time.deltaTime;
        }
    }

    public override bool isBusy()
    {
        return false;
    }

    public bool HordeAttack()
    {
        Debug.Log("AIForMainBase: Trying to launch horde attack...");
        List<UnitManager> playerUnits = GameManager.instance.GetAllUnitsForPlayer(GameManager.instance.gamePlayersParameters.myPlayerId);
        List<UnitManager> aiUnits = GameManager.instance.GetAllUnitsForPlayer(unitManager.Unit.Owner);

        UnitManager target = null;
        foreach (UnitManager um in playerUnits)
        {
            if (um.Unit.Owner != unitManager.Unit.Owner)
            {
                //check if the target is an derived class of BuildingManager
                if (um is BuildingManager)
                {
                    //with a chance of 50%, set it as target for all my units that can attack
                    if (Random.Range(0, 100) < 50)
                    {
                        target = um;
                        break;
                    }
                }
            }
        }
        
        Debug.Log("AIForMainBase[HordeAttack]: Found " + aiUnits.Count + " AI units, target is " + (target != null ? target.gameObject.name : "null"));

        if (target == null || aiUnits.Count <= 6) return false;

        int hordeSize = 0;
        foreach (UnitManager um in aiUnits)
        {
            if (um.Unit.AttackDamage > 0)
            {
                //with a chance of 90%, set the target
                if (Random.Range(0, 100) < 90)
                {
                    hordeSize++;

                    //make all the 
                    // um.Unit.SetTarget(target);
                    um.GetComponent<CharacterBT>().SetTarget(target);
                    Debug.Log("AIForMainBase[HordeAttack]: Unit " + um.gameObject.name + " attacking " + target.gameObject.name);
                }
            }
        }
        return hordeSize > 2;
    }
}
