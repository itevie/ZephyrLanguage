This folder contains all the built-in packages for Zephyr.
All packages are added into the global environment, unless they are using the NonDefaultPackage class.

Package's filenames that end with `Type` are special as they are directly used for interacting with types and adding functions on to them.
They are semi similar to JavaScript's prototypes.
For example, `AnyType.cs`, any function in that package is executable on any type, e.g., `"Hello World".length()`, this is the exact same as `Any.length("Hello World")`
