import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Comment } from './comment';
import { Game } from './game';

@Injectable()
export class DataService {

    private url = "/api/comments";
    private urlGame = "/api/games";

    constructor(private http: HttpClient) {
    }

    getComments() {
        return this.http.get(this.url);
    }

    getComment(id: number) {
        return this.http.get(this.url + '/' + id);
    }

    createComment(comment: Comment) {
        return this.http.post(this.url, comment);
    }
    updateComment(comment: Comment) {

        return this.http.put(this.url, comment);
    }
    deleteComment(id: number) {
        return this.http.delete(this.url + '/' + id);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////

    getGames() {
        return this.http.get(this.urlGame);
    }

    getGame(id: number) {
        return this.http.get(this.urlGame + '/' + id);
    }

    createGame(game: Game) {
        return this.http.post(this.urlGame, game);
    }
    updateGame(game: Game) {

        return this.http.put(this.urlGame, game);
    }
    deleteGame(id: number) {
        return this.http.delete(this.urlGame + '/' + id);
    }
}