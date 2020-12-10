using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Job", menuName = "ScriptableObjects/Job")]
public class JobScriptableObject : ScriptableObject
{
    public float jobTimeLimit;
    public JobStageInfo[] jobStages;
}
