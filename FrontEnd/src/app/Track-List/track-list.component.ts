import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { Song } from '../song';
import { SongService } from '../song.service';

@Component
({
    selector: 'app-track-list',
    templateUrl: './track-list.component.html',
    styleUrls: ['../app.component.css', './track-list.component.css']
})

export class TrackListComponent implements OnInit 
{
    @Input() songIn: Array<Song> = null;
    @Output() foundSongIndex = new EventEmitter<number>();
    public songs: Array<Song>;

    //Favourites List Fields
    @Input() deletable: boolean;
    @Output() deleteIndex = new EventEmitter<number>();

    constructor(public songService: SongService){}
  
    ngOnInit(): void 
    {
        if(this.songIn != null && this.songIn.length > 0)
            this.songs = this.songIn;
    }

    emitSongIndex(x: number): void
    {
        this.foundSongIndex.emit(x);
    }

    delete(x: number): void
    {
        this.deleteIndex.emit(x);
    }
}
