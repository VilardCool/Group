var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from '@angular/core';
import { DataService } from './data.service';
import { Game } from './game';
let GameComponent = class GameComponent {
    constructor(dataService) {
        this.dataService = dataService;
        this.game = new Game();
        this.tableModeGame = true;
    }
    ngOnInit() {
        this.loadGame();
    }
    loadGame() {
        this.dataService.getGames()
            .subscribe((data) => this.games = data);
    }
    saveGame() {
        if (this.game.id == null) {
            this.dataService.createGame(this.game)
                .subscribe((data) => this.games.push(data));
        }
        else {
            this.dataService.updateGame(this.game)
                .subscribe(data => this.loadGame());
        }
        this.cancelGame();
    }
    editGame(g) {
        this.game = g;
    }
    cancelGame() {
        this.game = new Game();
        this.tableModeGame = true;
    }
    deleteGame(g) {
        this.dataService.deleteGame(g.id)
            .subscribe(data => this.loadGame());
    }
    addGame() {
        this.cancelGame();
        this.tableModeGame = false;
    }
    likeGame(g) {
        g.amountOfLikes++;
    }
};
GameComponent = __decorate([
    Component({
        templateUrl: './game.component.html',
        providers: [DataService]
    })
], GameComponent);
export { GameComponent };
//# sourceMappingURL=game.component.js.map