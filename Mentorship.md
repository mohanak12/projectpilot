# Introduction #

The idea of this project is to help learn the following methods and technologies:
  * Test-driven design (TDD)
  * SOLID principles
  * Using mocks in test code (Rhino.Mocks specifically)
  * Using IoC containers (Windsor Castle specifically)
  * Using Selenium for Web testing

# Working #
I (Igor) will only give certain specifications to the developers (Jure, Marko,...). I will not directly change the code (other than when pair-programming with developers).

The specifications will come in small steps - there will not be a full-blown specification document right at the start. This is to simulate the reality of software projects - the specs are not clear at the start and they change during the lifetime of the project.

The approach to development should be to start with abstractions and not concentrate on implementation too early. First define interfaces that are needed to implement something and write unit tests that specify the behavior of implementations of those interfaces. Only then you start implementing interfaces - this is the practical TDD.

Once you start with this kind of approach, you will tend to design the architecture in a more open, manageable and testable way. Then you will see how the mocking and IoC containers come into the picture. You will also (hopefully) more easily understand the SOLID principles.

I recommend doing as much of pair-programming as possible.

The project will not necessarily result in some very useful product - this is not the primary goal. The main goal is to learn.


# Links #
  * TDD: http://en.wikipedia.org/wiki/Test-driven_development
  * SOLID principles: http://butunclebob.com/ArticleS.UncleBob.PrinciplesOfOod
  * Windsor Castle tutorial: http://wiki.bittercoder.com/Default.aspx?Page=ContainerTutorials&AspxAutoDetectCookieSupport=1
  * Rhino.Mocks introduction: http://aspalliance.com/1400_Beginning_to_Mock_with_Rhino_Mocks_and_MbUnit__Part_1.1
  * Selenium Remote Control tutorial: http://www.pghcodingdojo.org/index.php/Selenium_RC_Tutorial

# Task 1: EventMuncher #

This is a first sample "story" which will introduce concepts of IoC and mocking (working title is EventMuncher):

A system that will receive events from event sources and, based on filters and rules, publish these events.

**Event sources** could be anything. Some examples (not necessarily to be implemented): RSS feeds, event logs, text log files, Web pages, directories ...

**Filters** define whether the events should be processed or ignored. There could be various filters: based on the content of the event, based on time etc...

**Presenters** define how the received events are presented: as an e-mail, text log, HTML page etc.

There could be various **publishers**: email, RSS feed, SMS

The final result of this "project" should be a command line tool which uses an IoC container configuration file to process events and publishes them according to the configuration specification.

Later I will specify which event sources, publishers etc. to actually implement.

# Task 2: Painter, Stage 1 #

This should be a bit less abstract than EventMuncher.

**Description:** Create a painter component which will accept a list various geometrical shapes and draw them on the screen. The coordinates of the shapes have to be configurable. For this first stage implement drawing of lines, rectangles and circles.

**Notes:**
  * A sample shape list: Rectangle (10, 10, 200, 200), Circle (100, 100, 40), Circle (100, 130, 40), Line (10, 10, 20, 20)
  * As a demonstration, implement a **simple** WinForms application which will draw the shapes using GDI+. The shapes list can be hardcoded in the application (you don't need to implement any configuration reading in this stage).
  * The coordinates should be integers.

# Task 2: Painter, Stage 2 #
**New requirements:**
  * The developer should be able to add new types of shapes without changing the IPainter implementation (PainterGDI in your case).
  * By following this requirement, add some new shape types: Triangle, Pie slice

# Task 2: Painter, Stage 3 #
**New requirements:**
  * The user wants to specify color and pen width of each shape. But there are some catches:
    * She wants these parameters to be optional. If she doesn't specify a parameter, a default value should be used when painting.
    * She wants to be able to specify new parameters for shapes later without changing the interface.
HINT: maybe you could find some useful directions on how to solve this elegantly here: http://martinfowler.com/apsupp/properties.pdf

# Task 2: Painter, Stage 4 #
**New requirements:**
  * The user now wants to have some new filled shapes. We must provide her with a new shape parameter: fill color. We must extend the IDrawingEngine interface to allow drawing of filled elements. HINT: look how the interface for the [Graphics.FillPath](http://msdn.microsoft.com/en-us/library/system.drawing.graphics.fillpath.aspx) method looks like.

# Task 2: Painter, Stage 5 #
**New requirements:**
  * The user wants to be able to draw more complex drawings with reusable elements. Example: she wants to draw a house with windows. Each window consists of a frame and two lines, one horizontal and one vertical. Here are the catches:
    * She doesn't have the time to specify this again and again for each window
    * She also wants to be able to later change the design of a window in a single place.

HINT: maybe this could help? http://en.wikipedia.org/wiki/Composite_pattern

# Task 2: Painter, Stage 6 #
**New requirements:**
  * The user is tired of having to change the source code and recompile each time she wants to draw something different. Provide a way for her to change the drawing without recompiling your drawing software.

# Task 2: Painter, Stage 7 #
**New requirements:**
  * The user wants to have an Undo button in the application. After drawing something on the screen, she wants to be able to undo drawing steps (similarly to how undo functions works in MS Word). After clicking on the Undo button, the drawing should be redrawed without the undo-ed step.
HINT: look at the http://en.wikipedia.org/wiki/Command_pattern