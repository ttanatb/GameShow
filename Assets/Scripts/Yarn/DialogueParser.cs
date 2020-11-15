using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct TextModifier
{
    public enum Type
    {
        kNone,
        kShake,
        kWave,
    }

    public Type ModType;
    public int StartingIndex;
    public int Count;
}

public class DialogueParser : MonoBehaviour
{
    static readonly Dictionary<string, TextModifier.Type> kStringToModifierType =
        new Dictionary<string, TextModifier.Type>
        {
            { "shake", TextModifier.Type.kShake },
        };
    public static void Parse(string rawText, out string line, out string name, List<TextModifier> modifiers)
    {
        name = "";

        int indexOfColon = rawText.IndexOf(':');
        if (indexOfColon != -1)
        {
            name = rawText.Substring(0, indexOfColon);
            line = rawText.Substring(indexOfColon + 2, rawText.Length - indexOfColon - 2);
        }
        else
        {
            line = rawText;
        }

        var lineCopy = string.Copy(line);
        int removedCount = 0;
        for (int i = 0; i < lineCopy.Length; i++)
        {
            char c = lineCopy[i];

            // opening bracket
            if (c == '<')
            {
                // validate that there is a '>'
                int indexOfClosingBracket = lineCopy.IndexOf('>', i);
                if (indexOfClosingBracket == -1)
                {
                    Debug.LogErrorFormat("found opening bracket, but could not find closing bracket ({0})", line);
                    continue;
                }

                // check if opening or closing
                bool isOpeningModifier = lineCopy[i + 1] != '/';

                // index of when the mod string starts (<test> or </test> returns index of first t)

                int modifierNameIndex = isOpeningModifier ? i + 1 : i + 2;
                // get modifier type from name
                string modifierName = lineCopy.Substring(
                    modifierNameIndex,
                    indexOfClosingBracket - modifierNameIndex);
                TextModifier.Type type = TextModifier.Type.kNone;
                if (kStringToModifierType.ContainsKey(modifierName))
                    type = kStringToModifierType[modifierName];
                else
                    Debug.LogErrorFormat("unkown modifier named: {0}", modifierName);

                if (isOpeningModifier)
                {
                    // add new modifier to list
                    modifiers.Add(new TextModifier
                    {
                        ModType = type,
                        StartingIndex = i - removedCount,
                    });
                }
                else
                {
                    // specify where the modifier ends.
                    var index = FindLastModifierOfType(modifiers, type);
                    var newMod = modifiers[index];
                    newMod.Count = i - newMod.StartingIndex - removedCount;
                    modifiers[index] = newMod;
                }

                // Trim string
                var countToRemove = 2 + modifierName.Length + (isOpeningModifier ? 0 : 1);
                line = line.Remove(i - removedCount, countToRemove);
                removedCount += countToRemove;
            }
        }
    }

    static private int FindLastModifierOfType(List<TextModifier> modifiers, TextModifier.Type type)
    {
        for (int i = modifiers.Count - 1; i > -1; i--)
        {
            if (modifiers[i].ModType == type)
                return i;
        }

        return -1;
    }
}
