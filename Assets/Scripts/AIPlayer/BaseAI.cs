using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    [HideInInspector]public UnitManager unitManager;
    [HideInInspector]public List<SkillManager> skillManagers;
    public Unit unit;
    bool isAIControlled = true;

    public float castingCooldown = 0.2f;

    // Start is called before the first frame update
    public void Start()
    {
        unitManager = GetComponent<UnitManager>();
        if (unitManager == null)
        {
            Debug.LogError("No UnitManager found on " + gameObject.name);
            isAIControlled = false;
            return;
        }


        unit = unitManager.Unit;
        isAIControlled = unit.Owner != GameManager.instance.gamePlayersParameters.myPlayerId;
        skillManagers = unit.SkillManagers;
        for (int i = 0; i < skillManagers.Count; i++)
        {
            SkillManager sm = skillManagers[i];
            if (sm == null)
            {
                Debug.LogError("No SkillManager found on " + gameObject.name + " for skill index " + i);
            }
            else
            {
                sm.SetButton(null);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAIControlled)
        {
            RunAITick();
            castingCooldown -= Time.deltaTime;
            if (castingCooldown < 0) castingCooldown = 0;
        }
    }

    public virtual void RunAITick()
    {

    }   
    
    public virtual bool isBusy()
    {
        return false;
    }
}
