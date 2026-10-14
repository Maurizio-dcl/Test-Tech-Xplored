using UnityEngine;

namespace TestTechXplored.Core.Scripts.Board
{
    public sealed class BoardEntitiesView : MonoBehaviour
    {
        [SerializeField] private Transform entitiesContainer;

        public Transform EntitiesContainer => entitiesContainer;
    }
}
