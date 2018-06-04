import * as React from 'react';

interface RegisterHandleProps {
    onHandleSet: (handle: string) => Promise<boolean>
}

interface RegisterHandleState {
    handle: string,
    loading: boolean,
    error: string
}

export class RegisterHandle extends React.Component<RegisterHandleProps, RegisterHandleState> {
    constructor(props: any) {
        super(props);
        this.state = {handle: '', loading: false, error: ''};
        this.setHandle = this.setHandle.bind(this);
        this.handleHandleChange = this.handleHandleChange.bind(this);
    }

    public render() {
        return  <div>
            <div className='form-group'>
                <input className='form-control' 
                        value={this.state.handle} 
                        onChange={this.handleHandleChange} 
                        placeholder='What should we call you? (max 20 characters)'
                        maxLength={20}
                        autoFocus/>
            </div>
            <div className='text-danger'>{this.state.error}</div>
            <button className='btn btn-primary register-button' 
                onClick={this.setHandle}
                disabled={this.state.loading}>
                CONTINUE
            </button>
        </div>;
    }

    private setHandle() {
        this.setState({loading: true, error: ''});
        this.props.onHandleSet(this.state.handle)
            .catch(() => {
                this.setState({loading: false, error: 'Handle was taken or invalid'})
            });
    }

    private handleHandleChange(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({handle: event.target.value});
      }
}