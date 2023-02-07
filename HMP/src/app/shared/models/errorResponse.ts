export class ErrorResponse{
    public code: number;
    public httpCode: number;
    public message: string;

    constructor(init: Partial<ErrorResponse>) {
        Object.assign(this, init);
    }
}