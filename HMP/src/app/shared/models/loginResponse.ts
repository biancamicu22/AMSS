export class LoginResponse{
    public username: string;
    public email: string;
    public jwt: string;

    constructor(init: Partial<LoginResponse>) {
        Object.assign(this, init);
    }
}