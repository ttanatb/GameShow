using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class YarnSingleton : Singleton<YarnSingleton>
{
    public VariableStorage VariableStorage { get; private set; }

    public DialogueUI DialogueUI { get; private set; }

    public DialogueRunner DialogueRunner { get; private set; }

    private void Awake()
    {
        VariableStorage = GetComponentInChildren<VariableStorage>();
        DialogueUI = GetComponentInChildren<DialogueUI>();
        DialogueRunner = GetComponentInChildren<DialogueRunner>();
    }
}
