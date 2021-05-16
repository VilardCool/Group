export class Game {
    constructor(
        public id?: number,
        public title?: string,
        public picture?: string,
        public content?: string,
        public file?: string,
        public amountOfLikes?: number,
        public amountOfComments?: number,
        public amountOfReposts?: number) { }
}