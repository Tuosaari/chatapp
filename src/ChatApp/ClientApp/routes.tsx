import * as React from 'react';
import { Route } from 'react-router-dom';
import { LayoutContainer } from './containers/LayoutContainer';
import { ChatContainer } from './containers/ChatContainer';

export const routes = 
<LayoutContainer>
    <Route exact path='/' component={ ChatContainer } />
</LayoutContainer>;
