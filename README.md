# EasyFarm
EasyFarm is a general purpose farming tool for Final Fantasy XI

The program's main goal is to reduce the work needed for farming, allowing players to concentrate on the important aspects of the game. Rather than relying on job specific optimizations, its general design allows for adapting to many situations and enables support all job types. 

EasyFarm is free software, licensed under the GPLv3 software license which can be found [here](http://www.gnu.org/licenses/gpl-3.0-standalone.html) and in the project's root directory.

![EasyFarm GUI](http://i.imgur.com/pcrEm66.png)

# Features
* Mob filtering with regex expressions
* Aggressive mob detection
* Automatic self-healing
* Saves and restores session settings
* Customizable spell and ability usage
* (planned) Fields of Valor / Grounds of Valor support
* (planned) Trust Magic support
* (planned) Player Detection
* (planned) Item Usage Support 
* (planned) Dynamis Support (procing)
* (planned) Monstrosity Support
* (planned) Command Line Support (start / stop commands)

## Requirements
* Ashita or Windower Game Client
* [FFace.dll](http://delvl.ffevo.net/Lolwutt/FFACE4-Public/blob/master/FFACE.dll)
* [FFaceTools.dll](https://github.com/h1pp0/FFACETools_ffevo.net/tree/master/Binary)
* Resource Files (Optional)
* [Microsoft .NET Framework 4.5](https://www.microsoft.com/en-US/Download/details.aspx?id=30653)
* [Visual C++ Redistributable Packages for Visual Studio 2013](https://www.microsoft.com/en-us/download/details.aspx?id=40784)

    *Make sure your using the X86 version of the Visual Studio 2013 C++ Redistributable even if you have a 64 bit operating system.*

## Support
Use the [support issue](https://github.com/EasyFarm/EasyFarm/issues/130) for questions and feedback regarding the program. You can also send me an [email](MikeBartron@gmail.com), and I'll help you out as best I can. 

## Want to contribute?
Anyone can contribute to the project. Just make sure you've done your best to test the code you are changing and send me a pull request, and I'll add in your contribution!

## FAQ
##### Does the program detect aggro?
* Yes, the program can detect whether the player has aggro.

##### The program has caused my character to keeping running and I can't get him to stop. What should I do?
* Shut down the program, restart it and re-hit the start button.

##### Why is the program not targeting mobs correctly or not at all?
* Try turning off the in-game auto target feature and see if that fixes the problem.

##### Can you please add feature X?
* If the feature you request is in line with the goals of the program of course but adding additional features will take some time.
