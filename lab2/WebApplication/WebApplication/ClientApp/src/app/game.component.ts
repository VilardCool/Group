import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { Game } from './game';


@Component({
    templateUrl: './game.component.html',
    providers: [DataService]
})

export class GameComponent implements OnInit {

    game: Game = new Game();
    games: Game[];
    tableModeGame: boolean = true;

    constructor(private dataService: DataService) { }

    ngOnInit() {
        this.loadGame();
    }

    loadGame() {
        this.dataService.getGames()
            .subscribe((data: Game[]) => this.games = data);
    }

    saveGame() {
        if (this.game.id == null) {
            this.dataService.createGame(this.game)
                .subscribe((data: Game) => this.games.push(data));
        } else {
            this.dataService.updateGame(this.game)
                .subscribe(data => this.loadGame());
        }
        this.cancelGame();
    }
    editGame(g: Game) {
        this.game = g;
    }
    cancelGame() {
        this.game = new Game();
        this.tableModeGame = true;
    }
    deleteGame(g: Game) {
        this.dataService.deleteGame(g.id)
            .subscribe(data => this.loadGame());
    }
    addGame() {
        this.cancelGame();
        this.tableModeGame = false;
    }
    likeGame(g: Game) {
        g.amountOfLikes++;
    }
}