# Introduction to 2D Animation

This documentation contains information on how to quickly rig and animate a 2D character in Unity with the 2D Animation package and tools. The following package versions are only supported for the following Unity versions:

* __2D Animation package version 5.x.x__ - Unity 2020.2 or later

* __2D Animation package version 4.x.x__ - Unity 2020.1

* __2D Animation package version 3.x.x__ - Unity 2019.3

## 2D Animation and PSD Importer integration

You can use the __2D Animation package__ with the [PSD Importer](https://docs.unity3d.com/Packages/com.unity.2d.psdimporter@latest) package to easily import your character artwork from Photoshop into Unity for animation. The __PSD Importer__ is an Asset importer that supports the import of Adobe Photoshop .psb files into Unity and generating a Prefab made of Sprites based on the source file and its layers (see Adobe’s documentation on [Layer basics](https://helpx.adobe.com/photoshop/using/layer-basics.html)). 

The [.psb](https://helpx.adobe.com/photoshop/using/file-formats.html#large_document_format_psb)[ file format](https://helpx.adobe.com/photoshop/using/file-formats.html#large_document_format_psb) has identical functions as the more common Adobe .psd format, with additional support for much larger image sizes. See the [PSD Importer](https://docs.unity3d.com/Packages/com.unity.2d.psdimporter@latest/index.html) package documentation for more information about the importer’s features.

## Adobe Photoshop PSB format

For character animation with the __2D Animation package__, the __PSD Importer__ package is required. The PSD Importer package currently only imports the __Adobe Photoshop .psb format__, and does not import the Adobe .psd format. The .psb format has identical functions as .psd, and is able to support larger image sizes.

## Optional Performance Boost

You can improve the performance of the animated Sprite’s deformation at runtime by installing the [Burst](https://docs.unity3d.com/Packages/com.unity.burst@latest) and [Collections](https://docs.unity3d.com/Packages/com.unity.collections@latest) package from the [Package Manager](https://docs.unity3d.com/Manual/upm-ui.html). This allows the 2D Animation package to use Burst compilation to speed up Unity’s processing of Sprite mesh deformation.

![](images/SpriteSkin_inspect_exp.png)

With both packages installed, the Experimental __Enable batching__ setting becomes available in the [Sprite Skin component](#sprite-skin-component). As the implementation of the performance boost might contain bugs, you may switch back to the previous implementation by clearing the __Enable batching__ option.

The package has been tested with [Burst](https://docs.unity3d.com/Packages/com.unity.burst@latest) version 1.4.1 and [Collections](https://docs.unity3d.com/Packages/com.unity.collections@latest) version 0.9.0-preview.6.

## Sprite Skin component

When the character Prefab is brought into the Scene view, Unity automatically adds the __Sprite Skin__ component to any Sprite that have any [bone influences](SkinEdToolsShortcuts.md#bone-influences). This component is required for the bones deform the Sprite Meshes in the Scene view.
