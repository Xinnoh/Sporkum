using UnityEngine;

[CreateAssetMenu(fileName = "RouletteTemplate", menuName = "Roulette/Template")]
public class RouletteTemplate : ScriptableObject
{
    public int Normal = 5;
    public int Enemy = 5;
    public int Treasure = 3;
    public int Trap = 3;
    public int Other = 3;
    public int Extra = 0;
    public int Story = 1;
}
