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

# EventMuncher #

This is a first sample "story" which will introduce concepts of IoC and mocking (working title is EventMuncher):

A system that will receive events from event sources and, based on filters and rules, publish these events.

**Event sources** could be anything. Some examples (not necessarily to be implemented): RSS feeds, event logs, text log files, Web pages, directories ...

**Filters** define whether the events should be processed or ignored. There could be various filters: based on the content of the event, based on time etc...

**Presenters** define how the received events are presented: as an e-mail, text log, HTML page etc.

There could be various **publishers**: email, RSS feed, SMS

The final result of this "project" should be a command line tool which uses an IoC container configuration file to process events and publishes them according to the configuration specification.

Later I will specify which event sources, publishers etc. to actually implement.

# Links #
  * TDD: http://en.wikipedia.org/wiki/Test-driven_development
  * SOLID principles: http://butunclebob.com/ArticleS.UncleBob.PrinciplesOfOod
  * Windsor Castle tutorial: http://wiki.bittercoder.com/Default.aspx?Page=ContainerTutorials&AspxAutoDetectCookieSupport=1
  * Rhino.Mocks introduction: http://aspalliance.com/1400_Beginning_to_Mock_with_Rhino_Mocks_and_MbUnit__Part_1.1
  * Selenium Remote Control tutorial: http://www.pghcodingdojo.org/index.php/Selenium_RC_Tutorial