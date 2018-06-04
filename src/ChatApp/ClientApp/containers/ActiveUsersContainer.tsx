import * as React from 'react';
import 'isomorphic-fetch';
import { ActiveUsers } from '../components/ActiveUsers';
import { ChatService } from '../services/ChatService';

interface ActiveUsersContainerProps {
    handle: string,
    chatService: ChatService
}

interface ActiveUsersContainerState {
    users: Array<string>,
    loading: boolean
}

export class ActiveUsersContainer extends React.Component<ActiveUsersContainerProps, ActiveUsersContainerState> {
    constructor(props: any) {
        super(props);
        this.state = { users: [], loading: true };
        this.onNewUser = this.onNewUser.bind(this);
        this.onUserLeft = this.onUserLeft.bind(this);
    }

    componentDidMount() {
        fetch('api/v1/users/active')
            .then(response => response.json() as Promise<Array<string>>)
            .then(users => this.setState({users: users, loading: false}));
        this.props.chatService.registerNewUserListener(this.onNewUser);
        this.props.chatService.registerUserLeftListener(this.onUserLeft)
    }

    public render() {
        return <div className='d-flex flex-column align-self-stretch'>
            <ActiveUsers users={this.state.users} handle={this.props.handle}/>
        </div>;
    }

    private onNewUser(handle: string) {
        this.state.users.push(handle);
        this.setState({users: this.state.users});
    }

    private onUserLeft(handle: string) {
        const users = this.state.users.filter(u => u != handle);
        this.setState({users: users});
    }
}
