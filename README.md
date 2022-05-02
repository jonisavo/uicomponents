# UIComponents
#### A microframework for creating reusable components for Unity Editor's UIElements with dependency injection.

```c#
[Layout("MyComponent/MyComponent")]
[Stylesheet("MyComponent/MyComponent.style")]
[Stylesheet("Common")]
[Dependency(typeof(ICounterService), provide: typeof(CounterService))]
class MyComponent : UIComponent
{
    // The layout and stylesheets are loaded in the inherited
    // constructor. They are retrieved from Resources by default.
    
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

## Layouts and stylesheets

`[Layout]` allows specifying the path to a UXML file. The file will be
loaded automatically.

`[Stylesheet]` allows specifying paths to USS files. The files will be
loaded automatically. Unlike `[Layout]`, multiple `[Stylesheet]`
attributes can be used on a single UIComponent.

## Dependency injection

### Summary

Dependency injection requires an interface. Below is a simple example:

```c#
public interface ICounterService
{
    public void IncrementCount();
    public int GetCount();
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
**Each instance of CounterComponent receives the same instance of
CounterService.**

![CounterComponent in action](./Assets/Examples/Counter/counter.gif)

### Inheritance

UIComponents inherit dependencies. Such dependencies can be overridden
by specifying a different provider for them.

```c#
[Dependency(typeof(IStringDependency), provide: typeof(StringDependency)]
[Dependency(typeof(IScriptableObjectDependency), provide: typeof(HeroProvider)]
public class MyComponent : UIComponent {}

[Dependency(typeof(IScriptableObjectDependency), provide: typeof(VillainProvider)]
public class OtherComponent : MyComponent {}
```

### Testing

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

## Loading assets

### Resources

UIComponents load assets from Resources by default. To use a different
method, declare a different provider for the `IAssetResolver` dependency.
See an example below.

### AssetDatabase

UIComponents comes with `AssetDatabaseAssetResolver`, accessible via the
`UIComponents.Editor` namespace.

```c#
using UIComponents.Editor;

[Layout("Assets/Components/MyComponent.uxml")]
[Dependency(typeof(IAssetResolver), provide: typeof(AssetDatabaseAssetResolver))]
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
using UIComponents.Editor;

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
using UIComponents.Editor;

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

