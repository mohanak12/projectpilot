Files
=====
app.config 			- example configuration file the picks up the windsor ExampleConfiguration.config
windsor.xsd 			- the castle/windsor schema
ExampleConfiguration.config	- an example of a windsor configuration file with intellisense enabled


Adding intellisense to Windsor configurations
=============================================

Schema based intellisense is enabled by changing the <configuration> root node of the windsor xml configuration.

<configuration xmlns="MyWindsorSchema"
   xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
   xsi:schemaLocation="MyWindsorSchema file://S:\Common\Windsor\windsor.xsd">

</configuration


As you can see the schema does not have to live at an Http address - I am referencing mine from a mapped
local drive - file://S:\Common\Windsor\windsor.xsd.

Please copy the windsor.xsd file to your preferred location and update this location accordingly. 

e.g: If you put windsor.xsd in your c:\dev\castle folder the <configuration> would be...

<configuration xmlns="MyWindsorSchema"
   xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
   xsi:schemaLocation="MyWindsorSchema file://c:\dev\castle\windsor.xsd">


Enjoy!