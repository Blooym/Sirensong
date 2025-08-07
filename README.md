<div align="center">

<img src="./.assets/Icons/icon.png" alt="Sirensong Icon" width="15%" />

### Sirensong

**It is not recommended you use this library for now. It is outdated and consistently being tweaked.**

A collection of common utilities, helpers, extensions & UI styles for use in Dalamud plugins.

**[Issues](https://github.com/Blooym/Dalamud.Lib.Sirensong/issues) · [Pull Requests](https://github.com/Blooym/Dalamud.Lib.Sirensong/pulls)**

</div>

---

## About

Sirensong is a library that aids in the creation of Dalamud plugins by providing useful services, utility & extension methods and ImGui components.

As Sirensong is intended for use primarily within my projects, it contains many opinionated design choices that may not work for others. Right now Sirensong is not considered stable and, as such, may contain many breaking changes or bugs.

## Installing

Sirensong should be used as a submodule within your git repository. To do this, run the following command:

```
git submodule add https://github.com/Blooym/Sirensong.git
```

## Updating

To update to the latest version of the Sirensong submodule run the following command:

```
git submodule update --remote
```

Please be aware that updating your submodule may introduce breaking changes.

## Usage

To use Sirensong you must first initialize it. To do this call the initialize method within the `SirenCore` class. If you do not do this you will experience unstable behaviour when trying to use any functionality.

You are also required to dispose of Sirensong when you are finished using all of its functionality, although you don't need to dispose of any service class you did not explicitly instantiate with the `new` keyword.

An example of usage within a Dalamud plugin can be found below.

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

## Licence and Credits

Sirensong is licenced under the [GNU Affero General Public License v3.0](./LICENSE) and is maintained by [Blooym](https://github.com/Blooym). All contributions made to this project will also be licenced under the same licence.
