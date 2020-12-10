using UnityEngine;
using System;

[System.Serializable]
public class JobStageInfo
{
    public float progressDuration;
    public float completeExpirationDuration;
    public BuildingType jobBuilding;
    public Sprite stageImage;
}
