// Assets/Scripts/Firebase/ScriptableObjects/LocationEventSO.cs
using UnityEngine;

namespace CCG.Firebase
{
    [CreateAssetMenu(fileName = "LocationEvent", menuName = "CCG/Location Event", order = 0)]
    public class LocationEventSO : ScriptableObject
    {
        //Time is auto saved when sent
        
        [Header("Required")]
        [SerializeField] private string locationName;

        public string LocationName => locationName?.Trim();
    }
}