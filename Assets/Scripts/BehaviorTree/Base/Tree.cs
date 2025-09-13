using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        [SerializeField] private string info = "CharacterBT";
        private Node _root = null;

        protected void Start()
        {
            //write tree type to info
            info = this.GetType().ToString();
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root != null)
                _root.Evaluate();
        }

        public Node Root => _root;
        protected abstract Node SetupTree();
    }
}
