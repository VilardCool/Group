import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { Comment } from './comment';

@Component({
    selector: 'app',
    templateUrl: './comment.component.html',
    providers: [DataService]
})
export class CommentComponent implements OnInit {

    comment: Comment = new Comment();
    comments: Comment[];

    tableMode: boolean = true;

    constructor(private dataService: DataService) { }

    ngOnInit() {
        this.loadComments();
    }

    loadComments() {
        this.dataService.getComments()
            .subscribe((data: Comment[]) => this.comments = data);
    }

    save() {
        if (this.comment.id == null) {
            this.dataService.createComment(this.comment)
                .subscribe((data: Comment) => this.comments.push(data));
        } else {
            this.dataService.updateComment(this.comment)
                .subscribe(data => this.loadComments());
        }
        this.cancel();
    }
    editComment(p: Comment) {
        this.comment = p;
    }
    cancel() {
        this.comment = new Comment();
        this.tableMode = true;
    }
    delete(p: Comment) {
        this.dataService.deleteComment(p.id)
            .subscribe(data => this.loadComments());
    }
    add() {
        this.cancel();
        this.tableMode = false;
    }
    likeComment(p: Comment) {
        p.amountOfLikes++;
    }
}