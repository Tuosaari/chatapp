import * as React from 'react';
import { RegisterHandle } from '../components/RegisterHandle';
import { SignalRChatService } from '../services/SignalRChatService';
import { ChatService } from '../services/ChatService';

interface RegisterHandleContainerProps {
    chatService: ChatService,
    onHandleRegistered: (handle: string) => void
}

export class RegisterHandleContainer extends React.Component<RegisterHandleContainerProps> {

    constructor(props: any) {
        super(props);
        this.onHandleSet = this.onHandleSet.bind(this);
    }

    public render() {
        return <div className='justify-content-center register-entry'>
                <h1>Welcome to Chat!</h1>
                <RegisterHandle onHandleSet={this.onHandleSet}/>
            </div>;
    }

    private onHandleSet(handle: string): Promise<any> {
        return this.props.chatService.registerHandle(handle)
                .then(() => this.props.onHandleRegistered(handle));
    }
}
