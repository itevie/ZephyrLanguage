# Zephyr Language

Zephyr is a general purpose, interpreted language written in C#.

## CLI
Zephyr has many CLI features built-in for running projects, creating new projects and an in-built package manager.

To get help with the CLI, run `zephyr help`

### Running a project

To run a project / file, use `zephyr [filename]` or `zephyr run [filename]`
Note: the recommended file extension for a Zephyr file is `.zr`

### Installing a package

At the moment, there are no official permanent repository servers, though if there is you can use the following:

`zephyr install-package [package-name] [package-version] (--repository [url])`

package-name: The name of the package
package-version: The version of the package, or use `@latest` for the latest package
repository: The URL of the repository server, e.g. `--repository http://localhost:3000` or `-r http://localhost:3000`

### Creating a new project

Use `zephyr new` and it will guide you through creating a new project

## Found an issue or suggestion?

Contact me or send the issue as an Issue on here.
