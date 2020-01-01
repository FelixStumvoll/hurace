import React from 'react';
import styled, { ThemeProvider } from 'styled-components';
import { HuraceTheme } from './theme/HuraceTheme';
import { Navbar } from './components/Navbar';
import { Provider } from 'react-redux';
import { ConnectedRouter } from 'connected-react-router';
import { store, history } from './store/store';
import { Router } from './components/Router';

const PageContent = styled.div`
    display: grid;
    grid-template-areas: '.' 'main';
    height: 100%;
    grid-template-rows: ${props => props.theme.navHeight} 1fr;
`;

const MainContent = styled.main`
    grid-area: main;
    height: calc(100% - 20px);
    padding: 10px;
    overflow: hidden;
`;

export const App: React.FC = () => {
    return (
        <Provider store={store}>
            <ConnectedRouter history={history}>
                <ThemeProvider theme={HuraceTheme}>
                    <PageContent>
                        <Navbar />
                        <MainContent>
                            <Router />
                        </MainContent>
                    </PageContent>
                </ThemeProvider>
            </ConnectedRouter>
        </Provider>
    );
};
