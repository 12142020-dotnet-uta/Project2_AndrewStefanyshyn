import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './Home/home.component';
import { LoginComponent } from './Login/login.component';
import { RegisterComponent } from './Register/register.component';
import { ProfileComponent } from './Profile/profile.component';
import { ProfileEditComponent } from './Profile-Edit/profile-edit.component';
import { FriendListComponent } from './Friend-List/friend-list.component';
import { ChatComponent } from './Chat/chat.component';
import { FavouritesComponent } from './Favourites/favourites.component';
import { MusicComponent } from './Music/music.component';
import { MusicAddComponent } from './Music-Add/music-add.component';
import { SearchComponent } from './Search/search.component';
import { SearchUsersComponent } from './Search-Users/search-users.component';
import { GuardService } from './guard.service';


const routes: Routes = 
[
    { path: 'login', component: LoginComponent },
    { path: 'registration', component: RegisterComponent },
    { path: 'profile', canActivate: [GuardService], component: ProfileComponent },
    { path: 'profile/:id', component: ProfileComponent }, 
    { path: 'editProfile/:id', canActivate: [GuardService], component: ProfileEditComponent },
    { path: 'friends', canActivate: [GuardService], component: FriendListComponent },
    { path: 'chat/:aId/:bId', canActivate: [GuardService], component: ChatComponent },
    { path: 'favourites/:id', component: FavouritesComponent },
    { path: 'music/:id', component: MusicComponent },
    { path: 'upload', canActivate: [GuardService], component: MusicAddComponent },
    { path: 'search', component: SearchComponent },
    { path: 'usersearch', component: SearchUsersComponent },
    { path: '', component: HomeComponent }
];

@NgModule
({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule { }
