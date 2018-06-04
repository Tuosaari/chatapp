import * as React from 'react';

interface ActiveUsersProps {
    handle: string,
    users: Array<string>
}

export const ActiveUsers = ({users, handle}: ActiveUsersProps) => 
    <div className='active-users-list'>
        <div className='d-flex flex-row active-users-self'>
            <div className='active-users-self-icon d-flex justife-content-center'>
                <i className='far fa-user'/>
            </div>
            <div className='d-flex flex-column justife-content-center'>
                <b>{handle}</b>
                <small>you</small>
            </div>
        </div>
        <br/>
        <br/>
        {users.filter(u => u != handle).map(u => <p className='active-users-list-item' key={u}><i className='fas fa-user'/>&nbsp;&nbsp;{u}</p>)}
    </div>;
