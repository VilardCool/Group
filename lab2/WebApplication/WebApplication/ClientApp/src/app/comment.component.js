var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from '@angular/core';
import { DataService } from './data.service';
import { Comment } from './comment';
let CommentComponent = class CommentComponent {
    constructor(dataService) {
        this.dataService = dataService;
        this.comment = new Comment();
        this.tableMode = true;
    }
    ngOnInit() {
        this.loadComments();
    }
    loadComments() {
        this.dataService.getComments()
            .subscribe((data) => this.comments = data);
    }
    save() {
        if (this.comment.id == null) {
            this.dataService.createComment(this.comment)
                .subscribe((data) => this.comments.push(data));
        }
        else {
            this.dataService.updateComment(this.comment)
                .subscribe(data => this.loadComments());
        }
        this.cancel();
    }
    editComment(p) {
        this.comment = p;
    }
    cancel() {
        this.comment = new Comment();
        this.tableMode = true;
    }
    delete(p) {
        this.dataService.deleteComment(p.id)
            .subscribe(data => this.loadComments());
    }
    add() {
        this.cancel();
        this.tableMode = false;
    }
    likeComment(p) {
        p.amountOfLikes++;
    }
};
CommentComponent = __decorate([
    Component({
        selector: 'app',
        templateUrl: './comment.component.html',
        providers: [DataService]
    })
], CommentComponent);
export { CommentComponent };
//# sourceMappingURL=comment.component.js.map