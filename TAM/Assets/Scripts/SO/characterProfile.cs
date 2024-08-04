using System;
using UnityEngine;

[CreateAssetMenu(menuName = "profile/charProfile")]
public class characterProfile : ScriptableObject
{
    public characterProfileHolder charProfileHolder;
}

[Serializable]
public class characterProfileHolder
{
    [Range(0, 100)]
    public float speed;

    [Range(0, 1000)]
    public int maxHealth;

    [Range(0, 100)]
    public float jumpForce;

    [Range(0, 10)]
    public float range;
}