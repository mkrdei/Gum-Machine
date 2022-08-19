using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "ScriptableObjects/ColorPalette")]
class PaletteScriptable: ScriptableObject 
{
    [System.Serializable]    
    public class Entry 
    {
         public string name;
         public Color color;
    }

    public List<Entry> colors = new List<Entry>();

    public Color GetColors(string name) 
    { 
         var entry = colors.Find(c => c.name == name);
         if (entry != null) {
              return entry.color;
         }
         return Color.white;
    }
}