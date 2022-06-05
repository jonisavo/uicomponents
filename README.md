<h1 align="center">UIComponents</h1>

<p align="center">
    <img src="https://raw.githubusercontent.com/jonisavo/uicomponents/main/logo.png" alt="Logo" width="200px" height="200px" />
    <br />
    <i>A small front-end framework for Unity's UIToolkit.</i>
</p>

<p align="center">
	<a href="https://openupm.com/packages/io.savolainen.uicomponents/">
		<img src="https://img.shields.io/npm/v/io.savolainen.uicomponents?label=openupm&amp;registry_uri=https://package.openupm.com" alt="OpenUPM" />
	</a>
    <a href="https://github.com/jonisavo/uicomponents/actions/workflows/unity.yml">
        <img src="https://github.com/jonisavo/uicomponents/actions/workflows/unity.yml/badge.svg" alt="CI Status" />
    </a>
    <a href="https://codecov.io/gh/jonisavo/uicomponents">
      <img src="https://codecov.io/gh/jonisavo/uicomponents/branch/main/graph/badge.svg?token=A7DF04CF06" alt="Coverage Status" />
    </a>
</p>

<p align="center">
	<b>Note: UIComponents' API has not yet been fully stabilized. Expect breaking changes.</b>
</p>

## About

The goal of UIComponents is to ease the creation of reusable components when
working with Unity's new UIToolkit system. It offers ways to load UXML and USS
files automatically, and decouple your UI code from other systems via
dependency injection.

See an example of simple usage below.

## Simple usage

```c#
using UIComponents;

[Layout("MyComponent/MyComponent")]
[Stylesheet("MyComponent/MyComponent.style")]
[Stylesheet("Common")]
[Dependency(typeof(ICounterService), provide: typeof(CounterService))]
class MyComponent : UIComponent
{
    // The layout and stylesheets are loaded in the inherited
    // constructor. They are retrieved from Resources by default,
    // hence the lack of file extensions.
    
    private readonly ICounterService _counterService;
    
    public MyComponent()
    {
        // Will yield a CounterService.
        _counterService = Provide<ICounterService>();
    }
}
```

```c#
var container = new VisualElement();
container.Add(new MyComponent());
```

UIComponents are just VisualElements with some additional code in their
constructor for loading assets automatically.

## Installation

### With [OpenUPM](https://openupm.com/packages/io.savolainen.uicomponents/) (recommended)

```shell
openupm add io.savolainen.uicomponents
```

Alternatively, merge this snippet to your `Packages/manifest.json` file:

```json
{
    "scopedRegistries": [
        {
            "name": "package.openupm.com",
            "url": "https://package.openupm.com",
            "scopes": [
                "io.savolainen.uicomponents"
            ]
        }
    ],
    "dependencies": {
        "io.savolainen.uicomponents": "0.10.0"
    }
}
```

### With Git

Add this under `dependencies` in your `Packages/manifest.json` file:

```
"io.savolainen.uicomponents": "https://github.com/jonisavo/uicomponents.git#upm/v0.10.0"
```

This will install version 0.10.0.

To update, change `upm/v0.10.0` to point to the latest version.

## Layouts and stylesheets

`[Layout]` allows specifying the path to a UXML file. The file will be
loaded automatically. A component can have a single `[Layout]` attribute. It can be
inherited from a parent class.

```c#
[Layout("Assets/LayoutOne.uxml")]
public class UIComponentWithLayout : UIComponent {}

public class UIComponentWithSameLayout : UIComponentWithLayout
{
    // Uses Assets/LayoutOne.uxml
}

[Layout("Assets/LayoutTwo.uxml")
public class UIComponentWithOverriddenLayout : UIComponentWithLayout
{
    // Uses Assets/LayoutTwo.uxml
}
```

`[Stylesheet]` allows specifying paths to USS files. The files will be
loaded automatically. Unlike `[Layout]`, multiple `[Stylesheet]`
attributes can be used on a single UIComponent.

```c#
[Stylesheet("Assets/StylesheetOne.uss")]
[Stylesheet("Assets/StylesheetTwo.uss")]
public class UIComponentWithTwoStylesheets : UIComponent {}
        
[Stylesheet("Assets/StylesheetThree.uss")]
public class ChildComponent : UIComponentWithTwoStylesheets {}
```

Stylesheets will be applied to `ChildComponent` in the following order:
- `Assets/StylesheetOne.uss`
- `Assets/StylesheetTwo.uss`
- `Assets/StylesheetThree.uss`

This means that child components can override styles from their parents.

## Experimental features

`[Query]` is an experimental feature that allows for populating fields automatically.
It is accessible via the `UIComponents.Experimental` namespace.

```xml
<!-- Resources/MyLayout.uxml -->
<UXML xmlns:ui="UnityEngine.UIElements">
    <ui:Label name="my-label" text="Hello world!" />
    <ui:Foldout name="my-foldout">
        <ui:Label text="Foldout content" />
    </ui:Foldout>
</UXML>
```
```c#
using UIComponents;
using UIComponents.Experimental;

[Layout("MyLayout")]
public class MyComponent : UIComponent
{
    [Query("my-label")]
    public readonly Label MyLabel;
    
    [Query("my-foldout")]
    public readonly Foldout MyFoldout;
    
    public MyComponent()
    {
        MyLabel.text = "Goodbye world!";
        MyFoldout.Add(new Label("More content!"));
    }
}
```

## Dependency injection

### Summary

Dependencies injected into UIComponents act as singletons, meaning that every UIComponent
will have the same instance of the dependency.

Dependency injection requires an interface. Below is a simple example:

```c#
public interface ICounterService
{
    void IncrementCount();
    int GetCount();
}

public class CounterService : ICounterService
{
    private int _count;
    
    public void IncrementCount() => _count++;
    public int GetCount() => _count;
}
```

`CounterService` can be injected into components using the `[Dependency]` attribute:

```c#
using UIComponents;

[Dependency(typeof(ICounterService), provide: typeof(CounterService))]
public class CounterComponent : UIComponent
{
    private readonly ICounterService _counterService;   
    private readonly Label _countLabel;

    public CounterComponent()
    {
        _counterService = Provide<ICounterService>();
    
        _countLabel = new Label(_counterService.GetCount().ToString());
        Add(_countLabel);
    
        var incrementButton = new Button(IncrementCount);
        incrementButton.text = "Increment";
        Add(incrementButton);
    }

    private void IncrementCount()
    {
        _counterService.IncrementCount();
        _countLabel.text = _counterService.GetCount().ToString();
    }
}
```

This creates a component which can be used to increment a number.
**Since dependencies are singletons, every UIComponent, and in turn all CounterComponents receive the same instance of
CounterService.**

![CounterComponent in action](https://github.com/jonisavo/uicomponents/blob/main/Assets/Samples/Counter/counter.gif?raw=true)

### Get dependency safely

`Provide<T>` will throw a `MissingProviderException` if no providers exist for the dependency.
If you're unsure whether a provider exists, use `TryProvide<T>`:

```c#
if (TryProvide<ICounterService>(out var counterService))
    counterService.IncrementCount();
```

### Inheritance

UIComponents inherit dependencies. Such dependencies can be overridden
by specifying a different provider for them.

```c#
[Dependency(typeof(IStringDependency), provide: typeof(StringDependency))]
[Dependency(typeof(IScriptableObjectDependency), provide: typeof(HeroProvider))]
public class MyComponent : UIComponent {}

[Dependency(typeof(IScriptableObjectDependency), provide: typeof(VillainProvider))]
public class OtherComponent : MyComponent {}
```

### Testing

#### Switching dependencies

`DependencyInjector`, the class responsible for handling dependencies,
comes with the `SetDependency` static method.

```c#
private ICounterService _counterService;

[OneTimeSetUp]
public void OneTimeSetUp()
{
    _counterService = new MockCounterService();
    DependencyInjector.SetDependency<CounterComponent, ICounterService>(_counterService);
}
```

A mock version of a dependency can be switched in during a test. When `CounterComponent`
asks for `ICounterService`, it will receive the instance of `MockCounterService` created
in the `OneTimeSetUp` function.

The `DependencyScope` helper class is available under the `UIComponents.Utilities` namespace.
It is useful for switching dependencies temporarily.

```c#
[Dependency(typeof(ICounterService), provide: typeof(CounterService))]
public class CounterComponent : UIComponent {}
```
```c#
[Test]
public void It_Works()
{
    var service = new MockCounterService();
    using (new DependencyScope<CounterComponent, ICounterService>(service))
    {
        // MockCounterService will be provided for CounterComponent
    }
    
    // CounterService will be provided
}
```

#### Clearing dependencies

`DependencyInjector` also comes with the `ClearDependency` static method which can be used to remove
the instance of a dependency between tests.

```c#
[TearDown]
public void TearDown()
{
    DependencyInjector.ClearDependency<CounterComponent, ICounterService>();
    // Provide<ICounterService> inside CounterComponent will now throw a MissingProviderException
}
```

#### Restoring dependencies

Use `RestoreDefaultDependency` to restore the dependency to its original value as
specified in the `[Dependency]` attribute.

```c#
[TearDown]
public void TearDown()
{
    DependencyInjector.RestoreDefaultDependency<CounterComponent, ICounterService>();
    // Provide<ICounterService> inside CounterComponent will now yield
    // an instance of the type specified in the component's Dependency attribute.
}
```

If no `[Dependency]` attribute exists on the component, `RestoreDefaultDependency` will throw
an exception.

## Loading assets

### Resources

UIComponents load assets from Resources by default. To use a different
method, declare a different provider for the `IAssetResolver` dependency.
See an example below.

### AssetDatabase

UIComponents comes with `AssetDatabaseAssetResolver`, accessible via the
`UIComponents.Editor` namespace.

```c#
using UIComponents;
using UIComponents.Editor;

[Layout("Assets/Components/MyComponent.uxml")]
[Dependency(typeof(IAssetResolver), provide: typeof(AssetDatabaseAssetResolver))]
public class MyComponent : UIComponent {}
```

### Addressables

`AddressableAssetResolver` is accessible via the
`UIComponents.Addressables` namespace.

```c#
using UIComponents;
using UIComponents.Addressables;

[Layout("Assets/Components/MyComponent.uxml")]
[Dependency(typeof(IAssetResolver), provide: typeof(AddressableAssetResolver))]
public class MyComponent : UIComponent {}
```

You can create an abstract class with the overridden `IAssetResolver` dependency
and then inherit from that to apply the override to all of your components.

### Common asset paths

You will likely have your layout and stylesheet assets in one place. The
`[AssetPath]` attribute can be used instruct UIComponent to automatically
search for assets in those locations.

Here is an example of its usage alongside asset loading from `AssetDatabase`:

```c#
[AssetPath("Assets/UI/Components/MyComponent")]
[Layout("MyComponent.uxml")]
[Stylesheet("MyComponent.style.uss")]
[Dependency(typeof(IAssetResolver), provide: typeof(AssetDatabaseAssetResolver))]
public class MyComponent : UIComponent {}
```

`[AssetPath]` doesn't have much of an impact when applied to a single component.
However, like the `[Dependency]` attribute, it can be applied to
a parent class and inherited.

```c#
[AssetPath("Assets/UI/Components")]
[Dependency(typeof(IAssetResolver), provide: typeof(AssetDatabaseAssetResolver))]
public class BaseComponent : UIComponent {}

[Layout("MyComponent/MyComponent.uxml")]
[Stylesheet("MyComponent/MyComponent.style.uss")]
public class FirstComponent : BaseComponent {}

[Layout("SecondComponent/SecondComponent.uxml")]
[Stylesheet("SecondComponent/SecondComponent.style.uss")]
public class SecondComponent : BaseComponent {}
```

