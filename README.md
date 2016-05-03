[![Build status](https://ci.appveyor.com/api/projects/status/6o73j4hrbk02xroq?svg=true)](https://ci.appveyor.com/project/Mykezero/easyfarm)


### EasyFarm
EasyFarm is a general purpose farming tool for Final Fantasy XI. 

![EasyFarm GUI](http://i.imgur.com/pcrEm66.png)

#### Notice: 
The new version of the program uses a different library produced by [EliteMMO](http://www.elitemmonetwork.com/). 
If you are receiving an "Object reference not set to an instance of an object" message try running the requirements checker which can be found here: http://www.elitemmonetwork.com/forums/viewtopic.php?f=28&t=329

#### Downloads 
* [GitHub](https://github.com/EasyFarm/EasyFarm/releases): Release.zip contains the latest version. 

#### License
EasyFarm is free software produced under the GPLv3 license with the goal of producing a first class automation software for Final Fantasy XI that is freely accessible to everyone. 

#### Project Status
Development has slowed, and mostly happens on the weekends.

#### Features
* Advanced Mob Filtering 
* Aggro Detection
* Self Healing
* Persistent Settings
* Customizable Player Actions
* (planned) New Farming Modes (FoV, GoV, Dynamis) 
* (planned) Trust / Adventuring NPCs
* (planned) Detection Avoidance
* (planned) Inventory Control 

#### Requirements
* Ashita or Windower
* Resource Files (Optional)
* [Microsoft .NET Framework 4.5](https://www.microsoft.com/en-US/Download/details.aspx?id=30653)
* [Visual C++ Redistributable Packages for Visual Studio 2013]

    *Use the tool provided by EliteMMO to check for missing packages: http://www.elitemmonetwork.com/forums/viewtopic.php?f=28&t=329*
    
    *Make sure your using the X86 version of the Visual Studio 2013 C++ Redistributable even if you have a 64 bit operating system.*
    
#### Tutorials
Visit the [tutorials](https://github.com/EasyFarm/EasyFarm/blob/master/Documentation/readme.md) page for more information on setting up the program. 

#### Support
There's a few ways you can ask questions about the program or make suggestions to improve it. No option is preferred over the others so feel free to shoot me an email directly if you'd like! ^^;
* [EliteMMO Forums](http://www.elitemmonetwork.com/forums/viewtopic.php?f=10&t=394&sid=8152260e9de28e6e0a8319cae7701bd0)
* [Support Issue](https://github.com/EasyFarm/EasyFarm/issues/192) 

#### Want to contribute?
Anyone can contribute to the project. Just make sure you've done your best to test the code you are changing and send me a pull request, and I'll add in your contribution!

Contributions to the tutorial section are highly welcomed! You can submit a pull request if your tech savvy or post in the [support issue](https://github.com/EasyFarm/EasyFarm/issues/192) and I'll add in your contribution!!

#### Special Thanks!

* The FFEVO Team for producing the previous memory reading api this program could not operate without.

* Atom0s and EliteMMO for producing the current memory reading api this program could not operate without. 

* The Windower Team for producing the Windower client and resource files which make using the program a whole lot easier. 

* The DarkStar project for providing invaluable insight into the underlying workings of the game. 

* And of course the community which has made all this possible through their suggestions and feedback (and the occasional thank you) which makes working on this program a joy! 

#### FAQ

##### Does the program detect aggro?
* Yes and no. The program detects monsters in a aggressive state but cannot distinguish between aggressive and linking behaviors. 

##### My character will not stop running. What should I do?
1. Select your character under File > Select Character ...
1. Navigate to the Route's tab. 
2. Click the reset navigator button. 

##### Why is the program not targeting mobs correctly or not at all?
* Try turning off the in-game auto target feature.
