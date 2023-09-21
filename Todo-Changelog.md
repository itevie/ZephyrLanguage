# List of all things that I should do

## Stuff to do with packages
- [ ] Time & Date packages
- [ ] Add LINQ-type package
- [ ] Expand math module
	- [ ] ceil
	- [ ] abs
	- [ ] floor
	- [ ] factorial
	- [ ] trunc
	- [ ] sqrt
	- [ ] cos
	- [ ] sin
	- [ ] tan
	- [ ] tan2
	- [ ] pi
- [ ] Add environment variables to Process (like node's process.env)
- [ ] Path module
	- [ ] Get absolute path for something
	- [ ] Combine paths
- [ ] Expand file module
	- [ ] statistics
	- [ ] Open file for later editing (.open()) maybe
- [ ] More array types functions
	- [ ] Array.all(x => b);
- [ ] Spawning processes like node child_process
	- [ ] Be able to create a process object
	- [ ] Events will be on it, for example, `event string stdout`, `event string stderr` `event void exit` etc.
	- [ ] STDIN too
	- [ ] Then be able to execute it with `process.execute()` or something
	- [ ] Be able to provide args with `process.addArguments()` or something
	- [ ] Be able to set environment variables
	- [ ] Be able to set other things like cwd
	- [ ] Be able to add optional args
	- [x] Simpler version which just is `Process.executeFile()`
	- [x] Make it so the SpawnProcesses thing has to be enabled
- [ ] Cloning of objects and arrays without reference function
- [ ] Be able to parse integers, Integer.parse
- [ ] Expand random package
- [ ] CSV parser
- [ ] YML parser
- [ ] XML parser
- [ ] Put colors somewhere for like either console.color.red or console.color.red();
- [ ] CLI parser
	- [ ] Parses the CLI flags and returns an object
	- [ ] Allow use for custom verbs
	- [ ] Global flags
	- [ ] Defining syntax for a flag
	- [ ] Short versions of flags
	- [ ] Types for flags
	- [ ] Valid values for flags etc.
- [ ] Improve threading
	- [ ] Try make it more reliable
	- [ ] Function to get how many threads are alive

## Syntax
- [ ] Use `identifier` everywhere for names, not strings
- [ ] `Loop` keyword - identical to `while true`
- [ ] `until` keyword - reverse while
- [ ] Use `TypeExpression` instead of doing the types in the expression classes
- [ ] Fix ! breaking stuff, fix it not needing member expressions needing to be in a ()
- [ ] Be able to modify member expressions
- [ ] Re-do member expression evaluator
- [ ] Completely re-do type system
	- [ ] Add types to parameters
	- [ ] Better type checking, e.g. with function parameters e.g.: `function a(function<int, string>)` or something
	- [ ] Don't use Type AST, make it a new AST node which holds an identifier
	- [ ] Arrays as types `array[int]` `array[array[int]]` `int[]` `int[][]` or something like those
	- [ ] When calling things, allow to select which parameter it is, e.g. function(1, 2, 3, e=4);
	- [ ] Add `void` type
	- [ ] Typed function returns
	- [ ] Detect the return of a function before calling it
	- [ ] Type extensions
		- [ ] Objects which are defined as type extensions, so say as a variable object int and it contains all the IntegerType
		- [ ] Allow user to self define functions within these, say with func addOne(this int a, int b) or something
	- [ ] Expand the type checking for Inbuilt package
		- [ ] Instead of Int, double, float just allow Number and all of the 3 will be allowed
		- [ ] Allow types to by optional
		- [ ] Allow types to be nulled
		- [ ] Defining it's return type
		- [ ] Instead of giving an arg, give an object with the variables parameters
- [ ] Function overloading
- [ ] Global variables
- [ ] Regex strings
- [ ] Enums
- [ ] Disregard operator (_)
- [ ] Lambdas
	- [ ] Decide syntax for a lambda
	- [ ] Parse them
	- [ ] Make them work mostly same as a function
- [x] Swith statements
	- [ ] Inline switch statements too
	- [ ] Regex strings match 
- [ ] Immutable keyword
	- [ ] Add the keyword
	- [ ] Make native functions listen to it
	- [ ] Dissallow edited values, return a new value instead
- [ ] Continue keyword
- [ ] Assign to objects
- [ ] Python-like inexing like str[:3] or something
- [ ] Decorators
	- [ ] Find out what a decorator is
- [ ] Get around to adding the structs
- [ ] Template strings
- [ ] Define multiple variables in one line
- [ ] Indexing
	- [ ] Array indexing
	- [ ] Object indexing
	- [ ] Custom iterator on objects, perhaps add Symbols like [Symbol.Iterator]
- [ ] Delete keyword
- [ ] Directly be able to import .json files
- [ ] Hoisted variables
	- [ ] Add and parse variable
	- [ ] Add scope types to the environments, e.g. type = function
	- [ ] When used, make the environment find the closest function scope
- [ ] Documentation Comments
	- [ ] Parse them in lexer
	- [ ] Add properties to functions
	- [ ] Be able to access then with e.g. Function.getDocumentationComments(func).params.a.description
- [ ] Add classes
	- [ ] Create syntax for classes, make it good
	- [ ] Create public and private keywords for fields
	- [ ] Static fields
	- [ ] Add \_\_str\_\_ like python, method for converting the class into a string
	- [ ] Getters and setters

## Quality Of Life
- [ ] Detect and catch recursion errors so it doesn't produce c# stack overflow
- [ ] Make package manager / register stuff prettier and nicer
- [ ] Make errors better
	- [x] Add properties to the Exception class so it can be modified
	- [x] Make them more descriptive
	- [x] Have an enum full of different error codes
	- [ ] Have converters so its not like MemberExpression it's worded better, not how the class is named
	- [x] Store entire expressions inside of variables for example VariableDeclaration for where they were defined and errors can read this
	- [ ] Fully switch to new version
	- [x] "Defined here" in errors, e.g., cannot assign to constant variable: a (`a = 3`), defined here: `const a = 2`

## Everything else
- [ ] Access permissions similar to Deno
	- [ ] Whitelisting directories or files it can read from
	- [ ] Blacklist directories
	- [ ] Whitelist URLs it can access
	- [ ] Blacklist URLs
- [ ] Timers, it repeats function every so often
- [ ] Util functions
	- [ ] Solid type checker
		- [ ] Checks types
		- [ ] Checks number types "number" accepting all
		- [ ] Validate any
- [ ] Filename resolver, like node.js + C# combined: a.txt + ./a.txt .\a.txt ../a/a.txt etc.
- [ ] C# API for using Zephyr as an embedded script
- [ ] Fix all namespaces being wrong (some of them are)
- [ ] Package manger can only upload certain files, and checks what they are

## Advanced stuff
- [ ] Be able to import .zr files as inbuilt packages (so instead of cs functions it can load .zr files)
- [ ] API for creating custom C# modules, being able to import them like `import "raylib_bindings.dll"` or something
- [ ] Code prettifier, makes the code look nicer, preserving it's structure
- [ ] Code minifier
- [ ] Namspaces

# Tasks that are done (changelog)
While yes it could be seen as a changelog, I proably will forget to add everything I do
*Subtasks are not included - only when entire task is completed*

The dates mean, "these tasks have been completed ON this data or after this date up till the next done point"

## Done >21/09/23
- [x] Added parser and AST nodes for switch statement
- [x] Restructed some of the project

## Done >17/09/23
- [x] Break keyword
- [x] Piping values (1 >> add(2) >> mul(3) = 9) == mul(add(1, 2), 3)
- [x] varref function, returns a refernecne to a VARIABLE no a VALUE
	- [x] varref must be a operator cause native functions don't get VARIABLES
	- [x] Create "VariableValue" value
- [x] When loading NonDefaultPackages use zpehyr:package-name instead of whaatever it is now
- [x] Rename the package file to something else, not package.json
- [x] Update class names so its not stupid
- [x] Fix functions NEEDING to have parameters
- [x] Be able to modify the current working directory or the directory the application is running in with a flag, e.g. --dir=x