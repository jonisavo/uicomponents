

# [0.11.0](https://github.com/jonisavo/uicomponents/compare/v0.10.0...v0.11.0) (2022-06-05)


### Bug Fixes

* **DependencyInjector:** fix SetDependency exception message ([9090c74](https://github.com/jonisavo/uicomponents/commit/9090c7493e996d655d2c64b06507d6b936e6248b))


### Features

* **DependencyInjector:** add RestoreDefaultDependency method ([ef1952f](https://github.com/jonisavo/uicomponents/commit/ef1952ff93cb2b953ea4e86f7997b7ca4f79e443))

# [0.10.0](https://github.com/jonisavo/uicomponents/compare/v0.9.0...v0.10.0) (2022-06-03)


### Features

* **StylesheetAttribute:** apply stylesheets from parents upward ([a27f75e](https://github.com/jonisavo/uicomponents/commit/a27f75e9db84e11d3107e597b2e4abd65a7d0a52))


### BREAKING CHANGES

* **StylesheetAttribute:** Stylesheets defined in parent classes are now loaded first. This means that child classes can override styles from their parents.

# [0.9.0](https://github.com/jonisavo/uicomponents/compare/v0.8.0...v0.9.0) (2022-05-27)


### Features

* add experimental QueryAttribute ([1b7e0d5](https://github.com/jonisavo/uicomponents/commit/1b7e0d52146ad57b8aa573e4eef0fcf7c57c0641))
* **DependencyInjector:** add RemoveInjector static method for testing ([e514533](https://github.com/jonisavo/uicomponents/commit/e51453394a99d7dc3372a7d3b2fa12ff43fddb3b))


### Performance Improvements

* implement new cache for UIComponents ([6b90eb2](https://github.com/jonisavo/uicomponents/commit/6b90eb224b755aae703da6b4795871986cecb560))

# [0.8.0](https://github.com/jonisavo/uicomponents/compare/v0.7.0...v0.8.0) (2022-05-13)


### Bug Fixes

* **UIComponent:** add null check to layout loading function ([6359299](https://github.com/jonisavo/uicomponents/commit/6359299d4022fd9c898dc55e0452d610f0ce5441))


### Features

* add samples ([f4c0ccf](https://github.com/jonisavo/uicomponents/commit/f4c0ccf541e2d1327599853424f76ddb240c9147))

# [0.7.0](https://github.com/jonisavo/uicomponents/compare/v0.6.0...v0.7.0) (2022-05-11)


### Features

* add DependencyScope utility class ([dd8233a](https://github.com/jonisavo/uicomponents/commit/dd8233adfea60e1b9081abe72bbe7d643edc2033))
* **package.json:** add changelog, documentation and license URLs ([09e5555](https://github.com/jonisavo/uicomponents/commit/09e55554a88b4a199db03f063a776de26c2af7f7))

# [0.6.0](https://github.com/jonisavo/uicomponents/compare/v0.5.0...v0.6.0) (2022-05-05)


### Features

* move to UIComponents namespace from UIComponents.Core ([7efa0db](https://github.com/jonisavo/uicomponents/commit/7efa0db51f6936ad991f09dabe62759bcdd82cb9))


### BREAKING CHANGES

* All core code is now located in the UIComponents namespace, as opposed to UIComponents.Core.

# [0.5.0](https://github.com/jonisavo/uicomponents/compare/v0.4.0...v0.5.0) (2022-05-05)


* feat(UIComponent)!: make GetLayout and GetStyleSheets private ([4251611](https://github.com/jonisavo/uicomponents/commit/42516118aa3ff9d530cb8bad13f0ccf96c8a2bb3))


### Features

* add Addressables support ([#7](https://github.com/jonisavo/uicomponents/issues/7)) ([6abab8c](https://github.com/jonisavo/uicomponents/commit/6abab8cdb46b73f4c944098a7fd9aad94900668b))


### BREAKING CHANGES

* UIComponent's GetLayout and GetStyleSheets functions are no longer protected and virtual. Since they are called in UIComponent's constructor, derived classes would not have been fully initialized when the overridden functions are called.

# [0.4.0](https://github.com/jonisavo/uicomponents/compare/v0.3.0...v0.4.0) (2022-05-03)


### Features

* **DependencyInjector:** add ClearDependency method ([f621469](https://github.com/jonisavo/uicomponents/commit/f6214697441d3c05ab6b21b919f48f42a7847d9f))

# [0.3.0](https://github.com/jonisavo/uicomponents/compare/v0.2.1...v0.3.0) (2022-05-02)


### Features

* **UIComponent:** add TryProvide method ([c367311](https://github.com/jonisavo/uicomponents/commit/c3673114e6246010846ffa57619e171ad85279c9))

## [0.2.1](https://github.com/jonisavo/uicomponents/compare/v0.2.0...v0.2.1) (2022-05-02)


### Bug Fixes

* remove visibility modifier from interface members ([a51e751](https://github.com/jonisavo/uicomponents/commit/a51e7519e00846f7b31e6de9ef8d72c5985e4732))

# [0.2.0](https://github.com/jonisavo/uicomponents/compare/v0.1.0...v0.2.0) (2022-05-01)


### Bug Fixes

* **PathAttribute:** do not mutate Path instance variable in GetAssetPathForComponent ([cb01f27](https://github.com/jonisavo/uicomponents/commit/cb01f27d0b253b402adde8e9e36c35c1328aef2d))

### Features

* rework asset handling ([1c50b26](https://github.com/jonisavo/uicomponents/commit/1c50b2651250a8d6149db7e89ea7348b0c535764))
* rework dependency injection, create AssetDatabaseAssetLoader ([935703e](https://github.com/jonisavo/uicomponents/commit/935703e9b839fca7a7dc1b2d54f9c32dc753dcfd))
* allow specifying asset path with AssetPathAttribute ([336b6b9](https://github.com/jonisavo/uicomponents/commit/336b6b9140934e684496ba0b6fd225fd7cc1c910))
* add very basic dependency injection ([dc12f9d](https://github.com/jonisavo/uicomponents/commit/dc12f9d98422ad249ca84f6f9d398b65b3a7bdb2))
* **DependencyInjector:** add constructor for constructing injector using DependencyAttributes ([7b697b7](https://github.com/jonisavo/uicomponents/commit/7b697b7338f98e2339dfe2725aaadb3ee2658896))
* **DependencyInjector:** add TryProvide method ([9583b39](https://github.com/jonisavo/uicomponents/commit/9583b39147d1e6e08fcd602a9d083d56034b47f0))
* **DependencyInjector:** throw ArgumentNullException in SetDependency ([caa2e1b](https://github.com/jonisavo/uicomponents/commit/caa2e1b6c2f6a318aa2d33701f24b7079e75bd6e))
* **DependencyInjector:** throw exception in Provide<T> if no provider exists ([4ccebe5](https://github.com/jonisavo/uicomponents/commit/4ccebe50faac1ab17aec22c3816f1902c4c2e924))
* **examples:** add CounterComponent example ([bbb8f10](https://github.com/jonisavo/uicomponents/commit/bbb8f10401dfe62bf6a5f490c3b2bafa8f34df46))
* **UIComponent:** make AssetResolver public ([f2dc3b7](https://github.com/jonisavo/uicomponents/commit/f2dc3b7ae47dc59a527aa1093637f2fa9598ebcb))
* **UIComponent:** remove warning messages from when no layout or stylesheet files exist ([cf72d2d](https://github.com/jonisavo/uicomponents/commit/cf72d2de33bb2f4f09a5e9abbce06e40a50bed0d))


### BREAKING CHANGES

* The new ResourcesAssetResolver is
now used by default. AssetDatabaseAssetResolver has been
moved to the new UIComponents.Editor assembly.
* InjectDependencyAttribute has been renamed
to DependencyAttribute. The provider argument has been renamed
to provide. The majority of DependencyInjector has been reworked
completely.
* LayoutAttribute and StylesheetAttribute's
RelativeTo property has been removed.

## 0.1.0 [2022-04-25]

- Initial release