export class SprintModel{
    public firstName: string;
    public goal: string;
    public name: string;
    public id: string;
    public start_Date: Date;
    public end_Date: Date;

    constructor(init: Partial<SprintModel>) {
        Object.assign(this, init);
    }
}

/*

    get start_Date(): string{
        return this._start_Date.toLocaleDateString();
    }
    set start_Date(date: string){
        var dateParts = date.split("/");
        this._start_Date = new Date(+dateParts[2], parseInt(dateParts[1]) - 1, +dateParts[0]);
    }

    get end_Date(): string{
        return this._end_Date.toLocaleDateString();
    }
    set end_Date(date: string){
        var dateParts = date.split("/");
        this._end_Date = new Date(+dateParts[2], parseInt(dateParts[1]) - 1, +dateParts[0]);
    }

*/