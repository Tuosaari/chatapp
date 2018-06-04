import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { MessageList } from '../components/MessageList';
import { SignalRChatService } from '../services/SignalRChatService';
import 'isomorphic-fetch';
import { MessageComposer } from '../components/MessageComposer';
import { ChatService } from '../services/ChatService';

interface MessageComposerContainerProps {
    chatService: ChatService,
    handle: string;
}

export class MessageComposerContainer extends React.Component<MessageComposerContainerProps> {
    
    constructor(props: any) {
        super(props);
        this.state = { messages: [], loading: true };
        this.postMessage = this.postMessage.bind(this);
    }

    public render() {
        return <div>
            <MessageComposer handle={this.props.handle} onPostMessage={this.postMessage}/>
        </div>;
    }

    private postMessage(message: string): boolean {
        this.props.chatService.postMessage(message);
        return true;
    }
}
