using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : Singleton<Palette>
{
    [SerializeField]
    private PaletteScriptable paletteScriptable;
    
    
    public Color GetColor(string name) 
    {
        return paletteScriptable.GetColors(name);
    }
}
