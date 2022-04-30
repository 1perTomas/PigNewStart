
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField]
    [TextArea] private string[] dialogue;

    public string[] Dialogue => dialogue; //prevents code from outside writing to it, and can only read from it

}
