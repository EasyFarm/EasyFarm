### EasyFarm
EasyFarm is a general purpose farming tool for Final Fantasy XI. 

![EasyFarm GUI](http://i.imgur.com/pcrEm66.png)

#### Downloads 
* [GitHub](https://github.com/EasyFarm/EasyFarm/releases)

  Release.zip contains the latest version. 
  
* [AppVeyor](https://ci.appveyor.com/project/Mykezero/easyfarm/history) [![Build status](https://ci.appveyor.com/api/projects/status/6o73j4hrbk02xroq/branch/master?svg=true)](https://ci.appveyor.com/project/Mykezero/easyfarm/branch/master)

  EasyFarm\EasyFarm.zip contains the latest version. 

#### License
EasyFarm is free software produced under the GPLv3 license with the goal of producing a first class automation software for Final Fantasy XI that is freely accessible to everyone. 

#### Project Status
Development has slowed, and mostly happens on the weekends. The current goals for the project are: 

1. Reduce the program's maintenance cost.
2. Improve the program's core behavior. 

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
* [FFace.dll](http://delvl.ffevo.net/Lolwutt/FFACE4-Public/blob/master/FFACE.dll)
* [FFaceTools.dll](https://github.com/h1pp0/FFACETools_ffevo.net/tree/master/Binary)
* Resource Files (Optional)
* [Microsoft .NET Framework 4.5](https://www.microsoft.com/en-US/Download/details.aspx?id=30653)
* [Visual C++ Redistributable Packages for Visual Studio 2013](https://www.microsoft.com/en-us/download/details.aspx?id=40784)

    *Make sure your using the X86 version of the Visual Studio 2013 C++ Redistributable even if you have a 64 bit operating system.*

#### Support
Use the [support issue](https://github.com/EasyFarm/EasyFarm/issues/130) for questions and feedback regarding the program. You can also send me an [email](MikeBartron@gmail.com), and I'll help you out as best I can. 

#### Want to contribute?
Anyone can contribute to the project. Just make sure you've done your best to test the code you are changing and send me a pull request, and I'll add in your contribution!

#### FAQ
##### Does the program detect aggro?
* Yes and no. The program detects monsters in a aggressive state but cannot distinguish between aggressive and linking behaviors. 

##### My character will not stop running. What should I do?
1. Close, and restart the program. 
2. Clear the current route, and record a new waypoint. 
3. Start the program. 
You character will run and stop at the new waypoint. 

##### Why is the program not targeting mobs correctly or not at all?
* Try turning off the in-game auto target feature.
