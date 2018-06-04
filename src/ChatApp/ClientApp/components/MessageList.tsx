import * as React from 'react';
import * as moment from 'moment';
import { Message } from '../types/message';

interface MessageListProps {
    handle: string,
    messages: Array<Message>
}

interface MessageListState {
    tick: boolean
}

export class MessageList extends React.Component<MessageListProps, MessageListState> {
    scrollToElem: HTMLDivElement | null;

    constructor(props: any) {
        super(props);
        this.state = { tick: false };
        this.scrollToElem = null;
        this.scrollToBottom = this.scrollToBottom.bind(this);
        this.tick = this.tick.bind(this);
    }

    componentDidMount() {
        this.scrollToBottom();
        setInterval(this.tick, 5000);
    }

    componentDidUpdate() {
        this.scrollToBottom();
    }

    public render() {
        return <div className='message-list'>
            {this.props.messages.map(m => <div className='d-flex flex-column message-item' key={m.id}>
                <div><b>{m.handle == this.props.handle ? 'You' : m.handle}</b> - <small>{moment(m.timestamp).fromNow()}</small></div>
                <div>{m.message}</div>
            </div>)}

            <div style={{ float:"left", clear: "both" }}
                ref={(el) => { this.scrollToElem = el; }}>
            </div>
        </div>;
    }

    scrollToBottom = () => {
        if(this.scrollToElem != null) {
            this.scrollToElem.scrollIntoView({ behavior: "smooth" });
        }
    }

    tick = () => {
        this.setState({tick: !this.state.tick});
    }
}
