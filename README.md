# Rakis.Args - Parse Commandline arguments

Blame me for having a Unix background, but I simply got used to the way most GNU commandline software deals with arguments:

- A command is given as a list of strings separated by spaces. This usually causes some confusion with Windows' insistence at using spaces in directory and filenames, combined with the lack of an "escape character" such as the back-slash. So, "`Program Files`" can be typed as "`Program\ Files`" in practically any shell on Linux, but not on Windows' CMD. There you need to put quotes around the complete argument, so "`"C:\Program Files"`" will work, but "`C:\"Program Files"`" will not.

  Anyway, using Git Bash or WSL2 will save your day here, but then you're in Linux-land.

- A single dash ("`-`") starts one or more single-character options. If the option has an argument, it is the next argument in the line. If you happen to add multiple option-characters with an argument, you can get into trouble, but it is best to just continue with a fresh dash and a new set. So, if "`-a` and "`-c`" require arguments, don't try "`some-command -abcd arg1 arg2`" but instead use "`some-command -a arg1 -bc arg2 -d`".

- A double-dash ("`--`") starts an option with a spelled-out name. These names tend to use ["kebab-case"](https://en.wikipedia.org/wiki/Kebab_case) rather than ["camelCase"](https://en.wikipedia.org/wiki/Camel_case) to show where individual words are ending, although I see a number of "single-dash-camel-case" options in e.g. the "nuget" and "dotnet" CLI tools. For arguments to long option-names, the GNU standard is to use an equals sign and a value, without spaces around it. This is helped by the fact that Unix shells allow you to put quotes anywhere, so you can do "`--data="This is a long argument with a lot of text"`". On Windows you'll have to put the first quote before the first dash. Double-dash arguments cannot be combined, so each will need to become a separate argument.

  Generally all arguments are available as a double-dash variant with an easily recognizable name that explains its purpose. The most common ones then get a single-character version.

Some options are common enough to have become standard:

- "`-h`" (sometimes "`-?`", but not often) and "`--help`" ask for the program to print out a list of all possible options and their meaning. The program is not expected to do anything beyond that.
- "`--version`" asks for the program to print out version information and, again, not to do anything beyond that.
- "`-o <path>`" usually specifies an output filename.
- "`-f <path>`" usually specifies an input filename or "the" file.
- "`-v`" asks for the program to be (more) verbose, providing feedback on what it is doing.
- "`-d`" is sometimes used to request very verbose output, to debug a potential issue.
- If arguments (without an option specifier) are used for one or more input files, a single dash instead is taken to mean "read from standard input".
- An alternative use of a single dash is to tell the argument parser the explicit end of the list, allowing you to put arguments beyond that point that will start with a dash.

## Usage

Create an `ArgParser` object and pass it the array of arguments, then use this to add `Option`s. When done, call `Parse()` to obtain an `Args` object that can be queried.

```c#
  String[] args = { "--verbose", "-f", "file.txt" };

  var args = new ArgParser(args)
      .WithOption('v', "verbose")
      .WithOption("help")
      .WithOption("f", true)
      .Parse();

  if (args.Has("help")) {
    Console.WriteLine("Ask and you will be given...");
    return;
  }
  if (args.Has('v')) { // Both 'v' and "verbose" will work.
    Console.WriteLine("Starting program...");
  }
  using StreamReader f = new(args['f']);
  ... read file ...
...
}
```
