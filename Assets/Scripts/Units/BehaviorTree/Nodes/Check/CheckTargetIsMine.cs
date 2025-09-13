using UnityEngine;

using BehaviorTree;

public class CheckTargetIsMine: Node
{
    private int _myPlayerId;

    public CheckTargetIsMine(UnitManager manager) : base()
    {
        //patched. originally was always main player id
        _myPlayerId = manager.Unit.Owner;
    }

    public override NodeState Evaluate()
    {
        object currentTarget = Parent.GetData("currentTarget");
        if (currentTarget == null)
        {
            _state = NodeState.FAILURE;
            return _state;
        }
        UnitManager um = ((Transform)currentTarget).GetComponent<UnitManager>();
        if (um == null)
        {
            _state = NodeState.FAILURE;
            return _state;
        }
        _state = um.Unit.Owner == _myPlayerId ? NodeState.SUCCESS : NodeState.FAILURE;
        return _state;
    }
}
