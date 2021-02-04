import { User } from "./user";

export class FriendList
{
    public id: number;
    public friendId: number;
    public requestedFriendId: number;
    public toUsername: string;
    public fromUsername: string;
    public status: string;

    public friendListLink: number = 1000;

    constructor(f: number, t:number)
    {
        this.friendId = f;
        this.requestedFriendId = t;
        this.status = "Sent";
    }
}