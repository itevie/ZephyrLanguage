# Zephyr Language

Zephyr is a general purpose, interpreted language written in C#.

Using VSCode? Try the badly made VSCode syntax highlighting [VSCode Syntax Highlighter](https://github.com/itevie/zephyr-vscode-syntax-highlighting)

## CLI
Zephyr has many CLI features built-in for running projects, creating new projects and an in-built package manager.

To get help with the CLI, run `zephyr help`

### Running a project

To run a project / file, use `zephyr [filename]` or `zephyr run [filename]`
Note: the recommended file extension for a Zephyr file is `.zr`

### Installing a package

There IS an "official" package repository and it's: https://zephyrrepository.itevie.repl.co
It's the default repository URL, don't spam it or anything please, thanks
(Try the following server: https://zephyrrepository.itevie.repl.co)

`zephyr install-package [package-name] [package-version] (--repository [url])`

package-name: The name of the package
package-version: The version of the package, or use `@latest` for the latest package
repository: The URL of the repository server, e.g. `--repository http://localhost:3000` or `-r http://localhost:3000`

### Creating a new project

Use `zephyr new` and it will guide you through creating a new project

## Found an issue or suggestion?

Contact me or send the issue as an Issue on here.
