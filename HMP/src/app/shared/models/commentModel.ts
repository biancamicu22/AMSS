
export class CommentModel{
    public description: string;
    public id: string;
    public user_Id: string;
    public task_Id: string;
    public username: string;

    constructor(init: Partial<CommentModel>) {
        Object.assign(this, init);
    }
}