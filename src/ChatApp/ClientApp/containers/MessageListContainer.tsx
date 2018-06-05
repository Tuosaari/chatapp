import * as React from 'react';
import * as moment from 'moment';
import { RouteComponentProps } from 'react-router';
import { MessageList } from '../components/MessageList';
import { ChatService } from '../services/ChatService';
import 'isomorphic-fetch';
import { Message } from '../types/message';

interface MessageListContainerProps {
    handle: string,
    chatService: ChatService
}

interface MessageListContainerState {
    messages: Array<Message>,
    loading: boolean
}

export class MessageListContainer extends React.Component<MessageListContainerProps, MessageListContainerState> {

    constructor(props: any) {
        super(props);
        this.state = { messages: [], loading: true };
        this.onMessage = this.onMessage.bind(this);
        this.onNewUser = this.onNewUser.bind(this);
        this.onUserLeft = this.onUserLeft.bind(this);
    }

    componentDidMount() {
        this.props.chatService.getMessages().then((messages) => {
            this.setState({messages: messages.reverse().concat(this.state.messages), loading: false})
        });
        this.props.chatService.registerMessageListener(this.onMessage);
        this.props.chatService.registerNewUserListener(this.onNewUser);
        this.props.chatService.registerUserLeftListener(this.onUserLeft);
      }

    public render() {
        return <div className="d-flex message-list-container">
            <MessageList messages={this.state.messages} handle={this.props.handle}/>
        </div>;
    }

    private addMessage(message: Message) {
        this.state.messages.push(message);
        this.setState({messages: this.state.messages});
    }

    private onMessage(message: Message) {
        this.addMessage(message);
    }

    private onNewUser(handle: string) {
        const newUserMessage = `User ${handle} joined the chat`;
        const message = { id: '', handle: '-', message: newUserMessage, timestamp: moment().format()};
        this.addMessage(message);
    }

    private onUserLeft(handle: string) {
        const userLeftMessage = `User ${handle} left the chat`;
        const message = { id: '', handle: '-', message: userLeftMessage, timestamp: moment().format()};
        this.addMessage(message);
    }
}
