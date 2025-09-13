using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

[UnityEngine.RequireComponent(typeof(CharacterManager))]
public class CharacterBT : BehaviorTree.Tree
{
    CharacterManager manager;
    private TaskTrySetDestinationOrTarget _trySetDestinationOrTargetNode;

    private void Awake()
    {
        manager = GetComponent<CharacterManager>();
    }

    private void OnEnable()
    {
        EventManager.AddListener("TargetFormationOffsets", _OnTargetFormationOffsets);
        EventManager.AddListener("TargetFormationAI", _OnTargetFormationAI);
        EventManager.AddListener("TargetFormationPositions", _OnTargetFormationPositions);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener("TargetFormationOffsets", _OnTargetFormationOffsets);
        EventManager.RemoveListener("TargetFormationAI", _OnTargetFormationAI);
        EventManager.RemoveListener("TargetFormationPositions", _OnTargetFormationPositions);
    }

    private void _OnTargetFormationOffsets(object data)
    {
        List<UnityEngine.Vector2> targetOffsets = (List<UnityEngine.Vector2>)data;
        _trySetDestinationOrTargetNode.SetFormationTargetOffset(targetOffsets);
    }

    private void _OnTargetFormationAI(object data)
    {
        List<UnityEngine.Vector2> targetOffsets = (List<UnityEngine.Vector2>)data;
        _trySetDestinationOrTargetNode.SetFormationTargetOffsetAI(targetOffsets);
    }

    public void OnTargetAI(UnityEngine.Transform _targetTran)
    {
        List<UnityEngine.Vector2> targetOffsets = new List<UnityEngine.Vector2>() {
            UnityEngine.Vector2.zero
        };
        _trySetDestinationOrTargetNode.SetFormationTargetOffsetAI(targetOffsets, _targetTran);
    }

    private void _OnTargetFormationPositions(object data)
    {
        List<UnityEngine.Vector3> targetPositions = (List<UnityEngine.Vector3>)data;
        _trySetDestinationOrTargetNode.SetFormationTargetPosition(targetPositions);
    }

    public void StartBuildingConstruction(UnityEngine.Transform buildingTransform)
    {
        _trySetDestinationOrTargetNode.SetFormationTargetOffset(new List<UnityEngine.Vector2>() {
            UnityEngine.Vector2.zero
        }, buildingTransform);
    }

    public void StopBuildingConstruction()
    {
        manager.SetRendererVisibility(true);
        manager.SetIsConstructor(false);
        _trySetDestinationOrTargetNode.ClearData("currentTarget");
        _trySetDestinationOrTargetNode.ClearData("currentTargetOffset");
    }

    protected override Node SetupTree()
    {
        Node _root;

        // prepare our subtrees...
        _trySetDestinationOrTargetNode = new TaskTrySetDestinationOrTarget(manager);
        Sequence trySetDestinationOrTargetSequence = new Sequence(new List<Node> {
            new CheckUnitIsMine(manager),
            _trySetDestinationOrTargetNode,
        });

        Sequence moveToDestinationSequence = new Sequence(new List<Node> {
            new CheckHasDestination(),
            new TaskMoveToDestination(manager),
        });

        Selector attackOrBuildSelector = new Selector();
        if (manager.Unit.Data.attackDamage > 0)
        {
            Sequence attackSequence = new Sequence(new List<Node> {
                new Inverter(new List<Node>
                {
                    new CheckTargetIsMine(manager),
                }),
                new CheckUnitInRange(manager, true),
                new Timer(
                    manager.Unit.Data.attackRate,
                    new List<Node>()
                    {
                        new TaskAttack(manager)
                    }
                ),
            });

            attackOrBuildSelector.Attach(attackSequence);
        }

        CharacterData cd = (CharacterData)manager.Unit.Data;
        Sequence buildSequence = new Sequence(new List<Node> {
            new CheckTargetIsMine(manager),
            new CheckUnitInRange(manager, false)
        });
        Timer buildTimer = new Timer(
            cd.buildRate,
            new List<Node>()
            {
                new TaskBuild(manager)
            }
        );
        if (cd.buildPower > 0)
            buildSequence.Attach(buildTimer);
        attackOrBuildSelector.Attach(buildSequence);

        Sequence moveToTargetSequence = new Sequence(new List<Node> {
            new CheckHasTarget(),
            new Selector(new List<Node> {
                attackOrBuildSelector,
                new TaskFollow(manager),
            })
        });

        // ... then stitch them together under the root
        _root = new Selector(new List<Node> {
            new Parallel(new List<Node> {
                trySetDestinationOrTargetSequence,
                new Selector(new List<Node>
                {
                    moveToDestinationSequence,
                    moveToTargetSequence,
                }),
            }),
            new CheckEnemyInFOVRange(manager),
        });

        return _root;
    }

    public void SetTarget(UnitManager target)
    {
        _trySetDestinationOrTargetNode.SetTarget(target);
    }

    void OnDrawGizmos()
    {
        Transform currentTarget = _trySetDestinationOrTargetNode.GetData("currentTarget") as Transform;
        if (currentTarget != null)
        {
            UnityEngine.Vector3 targetPos = currentTarget.position;
            UnityEngine.Vector3 myPos = transform.position + UnityEngine.Vector3.up;
            UnityEngine.Gizmos.color = UnityEngine.Color.red;
            UnityEngine.Gizmos.DrawLine(myPos, targetPos);
        }

        UnityEngine.Vector3? destinationPoint = _trySetDestinationOrTargetNode.GetData("destinationPoint") as UnityEngine.Vector3?;
        if (destinationPoint != null)
        {
            UnityEngine.Vector3 destPos = (UnityEngine.Vector3)destinationPoint + UnityEngine.Vector3.up;
            UnityEngine.Vector3 myPos = transform.position + UnityEngine.Vector3.up;
            UnityEngine.Gizmos.color = UnityEngine.Color.green;
            UnityEngine.Gizmos.DrawLine(myPos, destPos);
        }
    }
}
