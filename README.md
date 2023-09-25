<h1 align="center">UIComponents</h1>

<p align="center">
    <img src="https://raw.githubusercontent.com/jonisavo/uicomponents/main/logo.png" alt="Logo" width="200px" height="200px" />
    <br />
    <i>A front-end framework for Unity's UIToolkit, powered by source generation.</i>
</p>

<p align="center">
	<a href="https://openupm.com/packages/io.savolainen.uicomponents/">
		<img src="https://img.shields.io/npm/v/io.savolainen.uicomponents?label=openupm&amp;registry_uri=https://package.openupm.com" alt="OpenUPM" />
	</a>
    <a href="https://github.com/jonisavo/uicomponents/actions/workflows/ci.yml">
        <img src="https://github.com/jonisavo/uicomponents/actions/workflows/ci.yml/badge.svg" alt="CI Status" />
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

UIComponents makes heavy use of code generation. It reduces boilerplate by generating vast amounts
of code for you.

## Requirements

UIComponents officially supports Unity 2021.3 or newer. Unity's `com.unity.roslyn` package
can be used to enable source generation in Unity 2020. Refer to the Installation section
below for more information.

## Example

```c#
using UnityEngine.UIElements;
using UIComponents;

[UxmlName("Counter")] // A UxmlFactory implementation is generated.
[Layout("CounterComponent/CounterComponent")]
[Stylesheet("CounterComponent/CounterComponent.style")]
[Stylesheet("Common")]
[Dependency(typeof(ICounterService), provide: typeof(CounterService))]
public partial class CounterComponent : UIComponent, IOnAttachToPanel
{
    // The layout and stylesheets are loaded automatically.
    // They are retrieved from Resources by default,
    // hence the lack of file extensions.
    
    // An UxmlTraits implementation is generated automatically for this class.
    [UxmlTrait]
    public string IncrementText = "Increment";
    
    // Queries are made after all assets have loaded.
    // The query calls are generated automatically for you.
    [Query("count-label")]
    public Label CountLabel;
    
    [Query("increment-button")]
    public Button IncrementButton;
    
    // An instance of CounterService is injected into this field
    // in the inherited constructor.
    [Provide]
    private ICounterService _counterService;
    
    // The OnInit method is called after all assets have loaded.
    // Any operations related to the DOM and stylesheets should
    // be done here.
    public override void OnInit()
    {
        CountLabel.text = _counterService.Count.ToString();
        IncrementButton.text = IncrementText;
    }
    
    // Event handlers are registered after all assets have loaded.
    // To listen for events, all you need to do is implement
    // a supported interface.
    public void OnAttachToPanel(AttachToPanelEvent evt)
    {
        CountLabel.text = _counterService.Count.ToString();
    }
}
```

Instantiation in code:

```c#
var container = new VisualElement();
container.Add(new CounterComponent() { IncrementText = "+1" });
```

Instantiation in UXML:

```xml
<Counter increment-text="+1" />
```

UIComponents are VisualElements with protected virtual methods which are overridden
via source generation. Those virtual methods are called when the UIComponent is first attached to a panel.

## Testing

The UIComponents package has been designed with testability in mind. The `UIComponents.Testing`
assembly contains the `TestBed<T>` helper class.

```c#
using UIComponents;
using UIComponents.Testing;
using NUnit.Framework;
using UnityEngine.TestTools;

[TestFixture]
public class CounterComponentTests
{
    private TestBed<CounterComponent> _testBed;
    private ICounterService _counterService;
    
    [SetUp]
    public void SetUp()
    {
        // A mocking framework like NSubstitute is recommended.
        // Here we don't use a mock at all.
        _counterService = new CounterService();
        _testBed = new TestBed<CounterComponent>()
            .WithSingleton<ICounterService>(_counterService);
    }
    
    [UnityTest]
    public IEnumerator It_Initializes_Count_Label_On_Init()
    {
        _counterService.Count = 42;

        var component = _testBed.Instantiate();
        // UIComponents start their initialization when they are first attached to a panel.
        // We can force the initialization by calling Initialize() manually.
        component.Initialize();
        // Wait until the component's assets have been loaded.
        yield return component.WaitForInitializationEnumerator();
        Assert.That(component.CountLabel.text, Is.EqualTo("42"));
    }
}
```

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
        "io.savolainen.uicomponents": "1.0.0-beta.7"
    }
}
```

### With Git

Add this under `dependencies` in your `Packages/manifest.json` file:

```
"io.savolainen.uicomponents": "https://github.com/jonisavo/uicomponents.git#upm/v1.0.0-beta.7"
```

This will install version 1.0.0-beta.7.

To update, change `upm/v1.0.0-beta.7` to point to the latest version.

### With .unitypackage

Download the latest `.unitypackage` from the [releases](https://github.com/jonisavo/uicomponents/releases) page.

To update, remove the existing files and extract the new `.unitypackage`.

### For Unity 2020

After installing UIComponents, install the `com.unity.roslyn` package. This enables source generation in Unity 2020.

Add this under `dependencies` in your `Packages/manifest.json` file:

```
"com.unity.roslyn": "0.2.2-preview"
```

You may need to restart Unity.

## Documentation

Refer to the [wiki](https://github.com/jonisavo/uicomponents/wiki) for documentation.

- [UxmlFactory & UxmlTraits generation](https://github.com/jonisavo/uicomponents/wiki/2.-UxmlFactory-&-UxmlTraits-generation): see
how you can use the `[UxmlName]` and `[UxmlTrait]` attributes to generate `UxmlFactory` and `UxmlTraits` implementations for
your VisualElements.
- [Layouts and stylesheets](https://github.com/jonisavo/uicomponents/wiki/3.-Layouts-and-stylesheets): see how UIComponents
loads layouts and stylesheets automatically, and how you can use `[Query]` to query for elements.
- [Asset loading](https://github.com/jonisavo/uicomponents/wiki/4.-Asset-loading): UIComponents loads assets from Resources
by default. See how you can use different methods.
- [Dependency injection](https://github.com/jonisavo/uicomponents/wiki/5.-Dependency-injection): UIComponents comes
with a simple dependency injection system. See how you can use it to decouple your UI code from other logic.
- [Event interfaces](https://github.com/jonisavo/uicomponents/wiki/6.-Event-interfaces): a list of interfaces
whose methods will be automatically registered as event callbacks.
- [Logging](https://github.com/jonisavo/uicomponents/wiki/7.-Logging): for when you want to use something other
than `Debug.Log`.
- [Experimental features](https://github.com/jonisavo/uicomponents/wiki/8.-Experimental-features): new features that
are subject to change.
