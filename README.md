<div align="center">

### Sirensong
A collection of common utilities used within Dalamud plugins.

**[Issues](https://github.com/BitsOfAByte/Sirensong/issues) · [Pull Requests](https://github.com/BitsOfAByte/Sirensong/pulls)**

</div>

## Important Notice

Sirensong is designed for use within my own plugins first and as such will contain a lot of opinionated design choices. Pull requests are welcome to add new features as long as they not designed to be used for cheating or botting.

Major breaking changes may be made at any time, by using Sirensong you are accepting that you may need to make a lot of changes to your plugin anytime you update your submodule. No stability guarantees are made.

## About

Sirensong is a collection of common utilities (a "commons library") for use within Dalamud plugins. It contains extension methods, ImGui components & elements, and other useful utilities that may be missing from Dalamud/FFXIVClientStructs.

## Installation

Sirensong must be used as a submodule within your git repository. To add Sirensong as a submodule, run the following command in your git repository:

```bash
git submodule add https://github.com/BitsOfAByte/Sirensong.git
```

Updating Sirensong:

```bash
git submodule update --remote
```

## Usage

You **must** initialize Sirensong before using any of its features. This is done by calling the `Initialize` method. It is recommended to do this within your plugin constructor.

You will also need to dispose of Sirensong when your plugin is unloaded. This is done by calling the `Dispose` method. It is recommended to do this within your plugin dispose method.

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

## License

This project is licensed under the [GNU Affero General Public License v3.0](./LICENSE) and is maintained by [BitsOfAByte](https://github.com/BitsOfAByte).
