using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton_Generic<AudioManager>
{
    protected override bool ShouldBeDestroyOnLoad() => false;

    static AudioManager ()
    {
        _useResources = false;
        _resourcesPath = "";
    }
}
