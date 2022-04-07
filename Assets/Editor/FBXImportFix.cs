using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FBXImportFix : AssetPostprocessor
{
    // This sets the scale factor of FBX imports to 1 rather than the default 0.01
    public void OnPostProcessModel()
    {
        ModelImporter importer = (ModelImporter)assetImporter;
        importer.globalScale = 1;
    }
}
