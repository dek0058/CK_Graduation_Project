# dotnet-localized-strings

A lightweight localization framework built on top of the NGettext library in .NET. This
framework is intended to be used with gettext style tooling although it is possible to
provide alternative implementations of the interfaces exposed by this framework.

```sh
$ yarn add rotorz/dotnet-localized-strings
```

This package is compatible with the [unity3d-package-syncer][tool] tool. Refer to the
tools' [README][tool] for information on syncing packages into a Unity project.

[tool]: https://github.com/rotorz/unity3d-package-syncer


## Setting up an environment with a single language domain

The included `LocalizedStringsPathsRepository` implementation discovers and loads
localized strings from files with a specified extension in the configured paths using the
`ILocalizedStringsLoader` instance that was provided to it on construction.

```csharp
// Paths in which to look for *.mo language files.
string[] paths = {
    // When multiple paths are supplied; strings are flattened such that latter strings
    // add to or shadow former strings.
    Path.Combine(applicationPath, "Languages"),
    // Sometimes it's useful to allow the user to override localizations in whole or even
    // just partially. It is not necessary to specify two paths; likewise you can specify
    // as many paths as desired.
    Path.Combine(userDataPath, "Languages"),
};

// Construct the language domain.
var loader = new LocalizedStringsLoaderMo();
var repository = new LocalizedStringsPathsRepository(paths, ".mo", loader);
var languageDomain = new LanguageDomain(repository);

// Load language domain for the desired active culture.
languageDomain.Load(activeCulture);

// Ideally expose localized strings via the `ILocalizedStrings` interface.
ILocalizedStrings strings = languageDomain;

// Present localizable text to the user.
Console.WriteLine(strings.Text("Hello, world!"));
```


## Contribution Agreement

This project is licensed under the MIT license (see LICENSE). To be in the best
position to enforce these licenses the copyright status of this project needs to
be as simple as possible. To achieve this the following terms and conditions
must be met:

- All contributed content (including but not limited to source code, text,
  image, videos, bug reports, suggestions, ideas, etc.) must be the
  contributors own work.

- The contributor disclaims all copyright and accepts that their contributed
  content will be released to the public domain.

- The act of submitting a contribution indicates that the contributor agrees
  with this agreement. This includes (but is not limited to) pull requests, issues,
  tickets, e-mails, newsgroups, blogs, forums, etc.
