import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { CommentComponent } from './comment.component';
import { GameComponent } from './game.component';
import { NotFoundComponent } from './not-found.component';

import { DataService } from './data.service';

const appRoutes: Routes = [
    { path: 'comment', component: CommentComponent },
    { path: '', component: GameComponent },
    { path: '**', component: NotFoundComponent }
];

@NgModule({
    imports: [BrowserModule, FormsModule, HttpClientModule, RouterModule.forRoot(appRoutes)],
    declarations: [AppComponent, GameComponent, CommentComponent, NotFoundComponent],
    providers: [DataService],
    bootstrap: [AppComponent]
})
export class AppModule { }