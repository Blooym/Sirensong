<div style="text-align: center;">

<img src="./.assets/Icons/icon.png" alt="Sirensong Icon" width="15%" />

### Sirensong

A collection of common utilities, helpers, extensions & UI styles for use in Dalamud plugins.

**[Issues](https://github.com/BitsOfAByte/Sirensong/issues) · [Pull Requests](https://github.com/BitsOfAByte/Sirensong/pulls)**

</div>

## Important Disclaimer

Sirensong is designed to be used within my own plugins first and foremost. As such, it will contain a fair amount of opinionated design choices that may not work for you. **Breaking changes may be made at any time.** Please be aware of this before using Sirensong in your own plugins.

## About

Sirensong is a collection of functionality that I have found myself reusing frequently and wanted to put into a centrialized location. It is designed to be used as a submodule within your own git repository and is not available as a NuGet package.

If you encounter any issues while trying to use Sirensong, please open an issue and I will try to address it as soon as possible. Please be aware that opinionated design choices might not be changed.

## Installing

Sirensong should be used as a submodule within your git repository. To add Sirensong as a submodule, run the in your git repository:

```bash
git submodule add https://github.com/BitsOfAByte/Sirensong.git
```

## Updating

To update Sirensong to the latest version, run the following command in your git repository:

```bash
git submodule update --remote
```

This will update the submodule to the latest version. You should be ready for breaking changes any time you do this.

## Usage

You **must** initialize Sirensong before using any of its features. This is done by calling the `Initialize` method. It is recommended to do this within your plugin constructor. If you do not initialize Sirensong, you will run into unpredictable or unstable behavior.

You also need to dispose of Sirensong when your plugin is unloaded. This is done by calling the `Dispose` method. It is recommended to do this within your plugin dispose method.

```csharp
using Sirensong;

public class MyPlugin : IDalamudPlugin
{
    public string Name => "MyPlugin";

    public MyPlugin(DalamudPluginInterface pluginInterface)
    {
        SirenCore.Initialize(pluginInterface, Name);
    }

    public void Dispose()
    {
        Sirensong.Dispose();
    }
}
```

## License and Credits

This project is licensed under the [GNU Affero General Public License v3.0](./LICENSE) and is maintained by [BitsOfAByte](https://github.com/BitsOfAByte). All contributions made to this project will also be licensed under the same license.
