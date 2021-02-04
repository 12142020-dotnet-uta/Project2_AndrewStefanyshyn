# Project2_AndrewStefanyshyn
This is a social website where users can upload original music to share with the public. They may also chat and make friends with other users. They may also listen to professionaly published music.
# Tech Stack
Backend: C# DotNetCore 5.0
Frontend: Angular 10
Database: MS SQL Server
# Installation
Open Backend/WhatsThatSong.sln in Visual Studio. Run the project.
Navigate a Command Prompt to Frontend/. Type the command "ng serve".
Open a browser and navigate to localhost:4200. You must login with a Spotify account if you wish to listen to published music. 
If the application does not work, you may have to open an unsafe browser. For Google Chrome, the command is "chrome.exe --disable-web-security  --user-data-dir=C:/chromeTemp"
# Usage
A user can register or login if they have an account. Accounts are unique by email.
A user can listen to Unpublished songs, which are hosted through SoundCloud. If a user wishes to listen to professionaly published music, they must login with a Spotify account.
A user can Search a song by title or lyrics. If the song exists in the database, they will be able to listen to it. If the song is available on Spotify, they will be able to listen to it.
A user can add songs to his favourites collection.
A user can upload original music. This music must be hosted on SoundCloud.
A user can make friends and chat with other users.
# License
This project is under the [MIT License](https://github.com/git/git-scm.com/blob/master/MIT-LICENSE.txt)
