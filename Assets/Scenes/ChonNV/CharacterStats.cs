using UnityEngine;

[CreateAssetMenu(menuName = "Game/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public string characterName;
    public int health;
    public int speed;
    public int damage;
}
