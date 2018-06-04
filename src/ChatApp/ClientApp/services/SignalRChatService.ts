import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { ChatService } from './ChatService';
import 'isomorphic-fetch';
import { Message } from '../types/message';

export class SignalRChatService implements ChatService {
    private connection: HubConnection;
    constructor() {
        const builder = new HubConnectionBuilder();
        builder.withUrl('/chat');
        this.connection = builder.build();
        this.connection.start();
    }

    registerHandle = (handle: string): Promise<any> => {
        return this.connection.invoke('registerHandle', handle);
    }

    postMessage = (message: string) => {
       this.connection.invoke('postMessage', message);
    }

    getMessages = () : Promise<Array<Message>> =>  {
        return fetch('api/v1/messages/all')
            .then(response => response.json() as Promise<Array<Message>>);
    }

    registerMessageListener = (onMessage: (message: Message) => void) => {
        this.connection.on('newMessage', onMessage);
    } 

    registerNewUserListener = (onNewUser: (handle: string) => void) => {
        this.connection.on('newUser', onNewUser);
    } 

    registerUserLeftListener =(onUserLeft: (handle: string) => void) => {
        this.connection.on('userLeft', onUserLeft);
    } 
}