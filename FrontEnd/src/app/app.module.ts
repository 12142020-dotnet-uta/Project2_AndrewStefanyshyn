import { BrowserModule } from '@angular/platform-browser';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { ChatComponent } from './Chat/chat.component';
import { FavouritesComponent } from './Favourites/favourites.component';
import { FriendListComponent } from './Friend-List/friend-list.component';
import { HomeComponent } from './Home/home.component';
import { LoginComponent } from './Login/login.component';
import { MusicComponent } from './Music/music.component';
import { MusicAddComponent } from './Music-Add/music-add.component';
import { RegisterComponent } from './Register/register.component';
import { ProfileComponent } from './Profile/profile.component';
import { ProfileEditComponent } from './Profile-Edit/profile-edit.component';
import { SearchComponent } from './Search/search.component';
import { SearchUsersComponent } from './Search-Users/search-users.component';
import { SongSummaryComponent } from './Song-Summary/song-summary.component';
import { TrackListComponent } from './Track-List/track-list.component';
import { LoginService } from './login.service';
import { GuardService } from './guard.service';
import { SongService } from './song.service';
import { FriendService } from './friend.service';
import { MessageService } from './message.service';
import { GeniusService } from './genius.service';
import { SpotifyService } from './spotify.service';

@NgModule
({
    declarations: 
    [
        AppComponent,
        ChatComponent,
        FavouritesComponent,
        FriendListComponent,
        HomeComponent,
        LoginComponent,
        MusicComponent,
        MusicAddComponent,
        RegisterComponent,
        ProfileComponent,
        ProfileEditComponent,
        SearchComponent,
        SearchUsersComponent,
        SongSummaryComponent,
        TrackListComponent
    ],
    imports: 
    [
        BrowserModule,
        TooltipModule.forRoot(),
        AppRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule
    ],
    providers: 
    [
        LoginService,
        GuardService,
        SongService,
        FriendService,
        MessageService,
        GeniusService,
        SpotifyService
    ],
    bootstrap: [AppComponent]
})

export class AppModule { }
