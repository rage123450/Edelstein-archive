# Edelstein [![CircleCI](https://circleci.com/gh/Kaioru/Edelstein.svg?style=svg)](https://circleci.com/gh/Kaioru/Edelstein)
A MapleStory Global v.95 server emulator written in C#.

**btw, this project is nowhere near complete.**

## 🔨 Building and Running
### Clone the repo
1. ```git clone https://github.com/Kaioru/Edelstein && cd Edelstein```
2. ```git submodule update --init --recursive```
### Build with your favourite tool/ide
1. On Visual Studio and Rider it should be pretty straightforward
2. Use ```dotnet build``` if not using an ide
### Running database migrations
1. ```cd src/Edelstein.Database```
2. ```cp Database.Example.json Database.json```
3. Edit database.json to the appropriate connection string
4. ```dotnet ef database update```

## ⭐️ Acknowledgements
* [Rebirth](https://github.com/RajanGrewal/Rebirth) - lot's of referencing from here.
* [Destiny](https://github.com/Fraysa/Destiny) - even more referencing from here.
* [@kwakery](https://github.com/kwakery) - for bein a nub, jk ily.

## 🚨 Disclaimer
* this project is purely educational.
* this project has not profitted in any way shape or form.
