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

See an example of usage below.

## Example usage

```c#
using UIComponents;

[Layout("MyComponent/MyComponent")]
[Stylesheet("MyComponent/MyComponent.style")]
[Stylesheet("Common")]
[Dependency(typeof(ICounterService), provide: typeof(CounterService))]
class MyComponent : UIComponent, IOnAttachToPanel
{
    // The layout and stylesheets are loaded in the inherited
    // constructor. They are retrieved from Resources by default,
    // hence the lack of file extensions.
    
    // Queries are made in the inherited constructor.
    [Query("count-label")]
    private readonly Label _countLabel;
    
    private readonly ICounterService _counterService;
    
    public MyComponent()
    {
        // Will yield a CounterService.
        _counterService = Provide<ICounterService>();
    }
    
    // Event handlers are made in the inherited constructor.
    // All you need to do is implement a supported interface.
    public void OnAttachToPanel(AttachToPanelEvent evt)
    {
        _countLabel.text = _counterService.Count.ToString();
    }
}
```

```c#
var container = new VisualElement();
container.Add(new MyComponent());
```

UIComponents are just VisualElements with some additional code in their
constructor for loading assets automatically, among other things.

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
        "io.savolainen.uicomponents": "0.19.0"
    }
}
```

### With Git

Add this under `dependencies` in your `Packages/manifest.json` file:

```
"io.savolainen.uicomponents": "https://github.com/jonisavo/uicomponents.git#upm/v0.19.0"
```

This will install version 0.19.0.

To update, change `upm/v0.19.0` to point to the latest version.

## Documentation

Refer to the [wiki](https://github.com/jonisavo/uicomponents/wiki) for documentation.

- [Layouts and stylesheets](https://github.com/jonisavo/uicomponents/wiki/2.-Layouts-and-stylesheets): see how UIComponents 
loads layouts and stylesheets automatically, and how you can use `[Query]` to query for elements.
- [Asset loading](https://github.com/jonisavo/uicomponents/wiki/3.-Asset-loading): UIComponents loads assets from Resources
by default. See how you can use different methods.
- [Dependency injection](https://github.com/jonisavo/uicomponents/wiki/4.-Dependency-injection): UIComponents comes
with a simple dependency injection system. See how you can use it to decouple your UI code from other logic.
- [Event interfaces](https://github.com/jonisavo/uicomponents/wiki/5.-Event-interfaces): a list of interfaces
whose methods will be automatically registered as event callbacks.
- [Logging](https://github.com/jonisavo/uicomponents/wiki/6.-Logging): for when you want to use something other
than `Debug.Log`.
- [Experimental features](https://github.com/jonisavo/uicomponents/wiki/7.-Experimental-features): new features that
are subject to change.
