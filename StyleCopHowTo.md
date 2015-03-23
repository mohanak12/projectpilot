## Introduction ##

Microsoft StyleCop is a free tool for enforcing Microsoft's coding style. It has been integrated into ProjectPilot's build and reports warnings about source code which does not conform to the coding style.

## Rules ##

Certain StyleCop rules have been turned off for ProjectPilot, things like:
  * "elements must be documented"
  * "file must have header"
  * "single line comments must not be followed by blank line"
  * ...
This was done in order to avoid excessive number of violations for the legacy code. But this doesn't mean that the code elements should not be documented, for example.

The rules exclusions are stored in the [Settings.StyleCop](http://code.google.com/p/projectpilot/source/browse/trunk/Settings.StyleCop) file. Exclusions can be edited by running the http://code.google.com/p/projectpilot/source/browse/trunk/EditStyleCopRules.bat file, but this should be done in agreement with the project lead.

In future StyleCop will be configured to **report errors instead of warnings** - developers will not be able to compile the project until any coding style violations are fixed.

## Integration With ReSharper ##
If you have ReSharper 4.1 installed (it has to be an official version, not one from nightly builds - these are currently not supported), you can install [StyleCop for ReSharper](http://www.codeplex.com/StyleCopForReSharper) add on. This enables you to view code style violations directly as you type the code, even before the code is compiled.