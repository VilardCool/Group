var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Injectable } from '@angular/core';
let DataService = class DataService {
    constructor(http) {
        this.http = http;
        this.url = "/api/comments";
        this.urlGame = "/api/games";
    }
    getComments() {
        return this.http.get(this.url);
    }
    getComment(id) {
        return this.http.get(this.url + '/' + id);
    }
    createComment(comment) {
        return this.http.post(this.url, comment);
    }
    updateComment(comment) {
        return this.http.put(this.url, comment);
    }
    deleteComment(id) {
        return this.http.delete(this.url + '/' + id);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////
    getGames() {
        return this.http.get(this.urlGame);
    }
    getGame(id) {
        return this.http.get(this.urlGame + '/' + id);
    }
    createGame(game) {
        return this.http.post(this.urlGame, game);
    }
    updateGame(game) {
        return this.http.put(this.urlGame, game);
    }
    deleteGame(id) {
        return this.http.delete(this.urlGame + '/' + id);
    }
};
DataService = __decorate([
    Injectable()
], DataService);
export { DataService };
//# sourceMappingURL=data.service.js.map