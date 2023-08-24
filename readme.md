# Zephyr Language

Zephyr is a general purpose, interpreted language written in C#.

Using VSCode? Try the badly made VSCode syntax highlighting [VSCode Syntax Highlighter](https://github.com/itevie/zephyr-vscode-syntax-highlighting)

## CLI
Zephyr has many CLI features built-in for running projects, creating new projects and an in-built package manager.

To get help with the CLI, run `zephyr help`

## Repository

The Zephyr program has a built-in package manager that interacts with an external repository server.
There is an "official" working one: https://zephyrrepository.itevie.repl.co
However, if you'd like to create your own, go to [Zephyr Repository Server](https://github.com/itevie/ZephyrRepositoryServer)
Help with the package manager CLI are below.

### Running a project

To run a project / file, use `zephyr [filename]` or `zephyr run [filename]`
Note: the recommended file extension for a Zephyr file is `.zr`

### Installing a package

There IS an "official" package repository and it's: https://zephyrrepository.itevie.repl.co
It's the default repository URL, don't spam it or anything please, thanks

`zephyr install-package [package-name] [package-version] (--repository [url])`

package-name: The name of the package
package-version: The version of the package, or use `@latest` for the latest package
repository: The URL of the repository server, e.g. `--repository http://localhost:3000` or `-r http://localhost:3000`

### Uploading a package

Uploading a package is very simple 1 line command.

First, make sure you're registered to the repository server, if not follow the registering section.

`zephyr upload-package -u [username] -p [password] (-r [url])`

Note:
1. You need to be registered
2. Zephyr must be running in a project folder with a package.json, if not it won't work.

### Registering

Registering to the repository server allows you to upload files etc.

To register run:

`zephyr register [username] [password] (-r [url])`

Usernames are alphanumeric (+ _) and 1-15 in length.
If the repository server is not the official one, use a random password for the password.

### Creating a new project

Use `zephyr new` and it will guide you through creating a new project

## Found an issue or suggestion?

Contact me or send the issue as an Issue on here.
