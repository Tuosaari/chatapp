import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { MessageList } from '../components/MessageList';
import { SignalRChatService } from '../services/SignalRChatService';
import 'isomorphic-fetch';
import { MessageListContainer } from './MessageListContainer';
import { MessageComposerContainer } from './MessageComposerContainer';
import { ActiveUsersContainer } from './ActiveUsersContainer';
import { RegisterHandleContainer } from './RegisterHandleContainer';
import { ChatService } from '../services/ChatService';

interface ChatContainerProps {
}

interface ChatContainerState {
    handle: string | null
} 

export class ChatContainer extends React.Component<ChatContainerProps, ChatContainerState> {
    private chatService: ChatService;

    constructor(props: any) {
        super(props);
        this.chatService = new SignalRChatService();
        this.state = { handle: null };
    }

    public render() {
        return <div className='d-flex justify-content-center chat-container'>
            { !this.state.handle && 
                <div className='d-flex justify-content-center register-container'>
                    <RegisterHandleContainer 
                        onHandleRegistered={handle => this.setState({handle: handle})}
                        chatService={this.chatService}/>
                </div>
            }
            { this.state.handle && 
                    <div className='d-flex flex-column align-self-start active-users-container'>
                        <ActiveUsersContainer chatService={this.chatService} handle={this.state.handle}/>
                    </div>
            }
            { this.state.handle && 
                    <div className='d-flex flex-column message-list-container'>
                        <MessageListContainer chatService={this.chatService} handle={this.state.handle}/>
                        <MessageComposerContainer handle={this.state.handle} chatService={this.chatService}/>
                    </div>
            }
        </div>;
    }
}
