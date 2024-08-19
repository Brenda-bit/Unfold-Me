using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptObject", menuName = "ScriptableObjects/EnemyScriptObject", order = 1)]
public class EnemyScriptObject : ScriptableObject
{
    public string enemyName;
    public float moveSpeed;
    public AnimationClip contactAnimation;
    public Vector3 spawnBackPlayer;
}