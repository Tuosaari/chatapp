import * as React from 'react';

interface MessageComposerProps {
    handle: string,
    onPostMessage: (message: string) => boolean
}

interface MessageComposerState {
    message: string,
    sending: boolean
}

export class MessageComposer extends React.Component<MessageComposerProps, MessageComposerState> {
    constructor(props: any) {
        super(props);
        this.state = {message: '', sending: false};
        this.postMessage = this.postMessage.bind(this);
        this.handleMessageChange = this.handleMessageChange.bind(this);
    }

    public render() {
        return  <div className='input-group mb-3'>
            <input 
                className='form-control' 
                value={this.state.message} 
                onChange={this.handleMessageChange} 
                placeholder='Type a message'
                autoFocus/>
            <div className="input-group-append">
                <button className="btn btn-outline-secondary" type="button" onClick={this.postMessage}>Send</button>
            </div>
        </div>;
    }

    private postMessage() {
        const result = this.props.onPostMessage(this.state.message);
        this.setState({message: '', sending: false});
    }

    private handleMessageChange(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({message: event.target.value});
      }
}