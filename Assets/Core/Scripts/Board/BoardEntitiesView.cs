using UnityEngine;
using UnityEngine.EventSystems;

namespace TestTechXplored.Core.Scripts.Board
{
    public sealed class BoardEntitiesView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Transform entitiesContainer;

        public Transform EntitiesContainer => entitiesContainer;

        public void OnPointerClick(PointerEventData eventData)
        {
            // This is just an hack to prevent clicks on each board entities to propagate to the underlying map                                                                                                                                                                                                     
            // container which is instead used to return entities back to their container 
        }
    }
}
