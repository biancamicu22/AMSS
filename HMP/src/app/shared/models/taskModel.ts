import { CommentModel } from "./commentModel";

export class TaskModel{
    public status: string;
    public description: string;
    public name: string;
    public id: string;
    public user_id: string;
    public sprint_id: string;
    public username: string;
    public comments: CommentModel[];

    constructor(init: Partial<TaskModel>) {
        Object.assign(this, init);
    }
}