# How to build

These instructions are *only* for building from the command line, which includes
compilation, test execution and packaging. This is the simplest way to build. It
also replicates the build on the Continuous Integration build server and is the
best indicator of whether a pull request will build.

You can also build the solution using Visual Studio 2019 or later, but this
doesn't provide the same assurances as the command line build.

At the time of writing the build is only confirmed to work on Windows.

## Prerequisites

The build requires that a few pieces of software be installed on the host
computer. We're somewhat aggressive about adopting new language features and the
like, so rather than specifying exactly which versions are required, we'll tend
toward "latest" or "at least" forms of guidance. If it seems you have an
incompatible version of the software, prefer to upgrade rather than downgrade.

Ensure that the following are installed:

1. A recent version of Visual Studio 2019 (currently this means 16.8 or later)
   or the Build Tools for Visual Studio 2019
2. The .NET Core 2.1 and 3.1 runtimes
3. The .NET Framework 4.6.1 or higher
4. The targeting pack for .NET Framework 4.5
5. An up-to-date version of the .NET 5.0 SDK (currently this means 5.0.100 or
   later)

## Building

Using a command prompt, navigate to your clone's root folder and execute
`build.cmd`.

After the build has completed, the build artifacts will be located in
`artifacts`.
