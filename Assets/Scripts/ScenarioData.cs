using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CustomDatas
{
    public Vector3 position;
    public Vector3 orientation;
}

[CreateAssetMenu(menuName = "New Scénario")]
public class ScenarioData : ScriptableObject
{
    public CustomDatas[] FirstWalls;

}

