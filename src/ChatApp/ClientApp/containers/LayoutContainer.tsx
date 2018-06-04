import * as React from 'react';

export interface LayoutProps {
    children?: React.ReactNode;
}

export class LayoutContainer extends React.Component<LayoutProps, {}> {
    public render() {
        return <div className='container-fluid main-container'>
                { this.props.children }
        </div>;
    }
}
