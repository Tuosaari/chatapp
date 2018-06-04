import { Message } from "../types/message";

export interface ChatService {
    registerHandle: (handle: string) => Promise<any>,
    postMessage: (message: string) => void,
    getMessages: () => Promise<Array<Message>>,
    registerMessageListener: (onMessage: (message: Message) => void) => void,
    registerNewUserListener: (onNewUser: (handle: string) => void) => void
    registerUserLeftListener: (onUserLeft: (handle: string) => void) => void
}