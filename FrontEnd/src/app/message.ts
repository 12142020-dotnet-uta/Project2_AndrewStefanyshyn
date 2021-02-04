import { User } from "./user";

export class Message
{
    public id: number;
    public toUserId: number;
    public fromUserId: number;
    public content: string;
    public fromUserName: string

    constructor(a: number, b: number, c: string, f: string)
    {
        this.toUserId = a;
        this.fromUserId = b;
        this.content = c;
        this.fromUserName = f;
    }
}