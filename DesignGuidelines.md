## Introduction ##
In general we follow Microsoft's design guidelines. We use StyleCop and FxCop tools as part of the build, they enforce some of these guidelines. See the StyleCopHowTo on some basic instructions in using the StyleCop.

## Some Coding Style Guidelines ##
This page describes only some of the most important coding style guidelines, for the rest of them please consult the links below.

### Order Of Things In Classes/Interfaces ###
The order must follow these rules (in order of importance):
  1. First the public stuff, then protected, then private
  1. First constructors, then properties, then methods, then fields
  1. Sorted alphabetically

### Miscellaneous ###
  * Keep each class/interface in its own file.
  * Use 4 spaces instead of tabs in VisualStudio. Otherwise changes of formatting will be shown as real changes.
  * Write meaningful XML comments. You can use [Ghostdoc](http://www.roland-weigelt.de/ghostdoc/) to help you write them.


## Links ##
  * [Design Guidelines for Developing Class Libraries](http://msdn.microsoft.com/en-us/library/ms229042.aspx)
  * [Internal Coding Guidelines](http://blogs.msdn.com/brada/articles/361363.aspx)
  * [Microsoft StyleCop](http://code.msdn.microsoft.com/sourceanalysis)